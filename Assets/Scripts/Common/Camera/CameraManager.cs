using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    [Header("CameraAttribute")]
    //캐릭터를 따라다니는 모드인지 아닌지를 판단합니다.
    public bool playMode = true;

    //카메라 옵션입니다.
    public float followSpeed = 9f;          //따라가는 속도입니다.
    public float mouseSpeed = 2f;           //회전하는 속도입니다.
    public float controllerSpeed = 7f;

    //카메라가 도는 속도입니다.
    public float turnSmoothing = 0.1f;
    //위 아래를 보는 각도의 한계입니다.
    public float minAngle = -35f;
    public float maxAngle = 35f;

    //메인카메라의 입니다.
    public GameObject mainCam;
    //서브카메라의 입니다.
    public GameObject subCam;

    //지정된 정보입니다.
    public Transform playerTr;
    public Transform target;
    public Transform pivot;

    //현재 보는 각도입니다.
    public float lookAngle;
    public float tiltAngle;

    //카메라의 정보입니다
    private Transform _camTrans;
    private float _smoothX;
    private float _smoothY;
    private float _smoothXVelocity;
    private float _smoothYVelocity;
    
    public void Init(Transform t)
    {
        //타겟과 카메라 위치를 초기화한다.
        target = t;
        _camTrans = Camera.main.transform;
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
            _smoothX = Mathf.SmoothDamp(_smoothX, h, ref _smoothXVelocity, turnSmoothing);
            _smoothY = Mathf.SmoothDamp(_smoothY, v, ref _smoothYVelocity, turnSmoothing);
        }
        else
        {
            _smoothX = h;
            _smoothY = v;
        }
        //x회전 입니다.
        lookAngle += _smoothX * targetSpeed;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);

        //y회전 입니다.
        tiltAngle -= _smoothY * targetSpeed;
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

    //카메라가 멈춰있을때 상태를 처리합니다.
    public void PauseCamaraChangeON(GameObject objectTemp)
    {
        Vector3 temp;
        if (playMode == true)
        {
            //카메라의 위치를 조정합니다.
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
