using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager m_instance;

    //싱글톤을 접근할때 사용합니다.
    public static CameraManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<CameraManager>();
            }
            return m_instance;
        }


    }

    //캐릭터를 따라다니는 모드인지 아닌지를 판단합니다.
    public bool playMode = true;

    //캐릭터를 따라오는 속도입니다.
    public float followSpeed = 9f;
    public float mouseSpeed = 2f;
    public float controllerSpeed = 7f;

    //플레이어를 저장해둡니다.
    public Transform playerTr;
    //카메라의 타겟입니다.
    public Transform target;
    //카메라의 pivot점입니다.
    public Transform pivot;
    [HideInInspector]
    //카메라의 좌표입니다
    public Transform camTrans;
    //메인카메라의 입니다.
    public GameObject mainCam;
    //서브카메라의 입니다.
    public GameObject subCam;
    
    //카메라가 도는 속도입니다.
    float turnSmoothing = 0.1f;
    //위 아래를 보는 각도의 한계입니다.
    public float minAngle = -35f; 
    public float maxAngle = 35f;  

    float smoothX;
    float smoothY;
    float smoothXVelocity;
    float smoothYVelocity;
    public float lookAngle;
    public float tiltAngle;


    public void Init(Transform t)
    {
        //타겟과 카메라 위치를 초기화한다.
        target = t;
        camTrans = Camera.main.transform;
        //pivot = camTrans.parent;
    }

    public void FixedTick(float d)
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        float c_h = Input.GetAxis("RightAxis X");
        float c_v = Input.GetAxis("RightAxis Y");

        float targetSpeed = mouseSpeed;


        if (c_h != 0 || c_v != 0)
        {
            h = c_h;
            v = c_v;
            targetSpeed = controllerSpeed;
        }

        FollowTarget(d);
        HandleRotations(d, v, h, targetSpeed);
        
    }


    public void FixedTick(float d, float h, float v)
    {
        float targetSpeed = mouseSpeed;

        FollowTarget(d);
        HandleRotations(d, v, h, targetSpeed);
    }

    void FollowTarget(float d)
    {
        //타겟을 따라가는 카메라 기능입니다.
        float speed = d * followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
        transform.position = targetPosition;
    }

    void HandleRotations(float d, float v, float h, float targetSpeed)
    {
        //카메라의 회전을 책임집니다.
        if (turnSmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXVelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYVelocity, turnSmoothing);
        }
        else
        {
            smoothX = h;
            smoothY = v;
        }
        //x회전 입니다.
        lookAngle += smoothX * targetSpeed;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);

        //y회전 입니다.
        tiltAngle -= smoothY * targetSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }

    public void InitSight(Vector3 angle)
    {
        //x회전 입니다.
        lookAngle += angle.y;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);

        //y회전 입니다.
        tiltAngle += angle.x;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }

    public void PauseCamaraChangeON(GameObject objectTemp)
    {
        Vector3 temp;
        if (playMode == true)
        {
            target = playerTr;
            temp = objectTemp.transform.position;

            for (int i=0;i<5;i++)
                temp = temp + objectTemp.transform.forward;
            temp -= objectTemp.transform.right;
            temp += objectTemp.transform.up;

            subCam.transform.position = temp;

            temp = objectTemp.transform.position - objectTemp.transform.right;
            temp += objectTemp.transform.up;
            subCam.transform.LookAt(temp);

            subCam.SetActive(true);
            mainCam.SetActive(false);

            playMode = false;
        }
    }

    public void PauseCamaraChangeOFF(GameObject objectTemp)
    {
        if (playMode == false)
        {
            target = target.transform;
            subCam.SetActive(false);
            mainCam.SetActive(true);

            playMode = true;
        }
    }
}
