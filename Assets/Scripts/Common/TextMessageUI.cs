using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMessageUI : MonoBehaviour
{
    public bool player = false;

    //canvas를 렌더링 하는 카메라
    private Camera uiCamera;
    //UI용 최상위 캔버스
    public Canvas canvas;
    //부모 rectTransform 컴포넌트
    private RectTransform rectParent;
    //자신 RectTransform 컴포넌트 
    private RectTransform rectTrans;

    //이미지의 위치를 조절할 오프셋
    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Vector3 targetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();

        rectTrans = GetComponent<RectTransform>();

        offset.x = Random.Range(-0.5f,0.5f);

        Destroy(this.gameObject, 3.0f);
    }

    private void LateUpdate()
    {
        offset.y += 0.02f;
        //월드 좌표를 스크린 좌표로 변환
        var screenPos = Camera.main.WorldToScreenPoint(targetPosition + offset);

        //카메라의 뒷쪽 영역일때 좌푯값 보정
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        //RectTransform 좌푯값을 전달받을 함수
        var localPos = Vector2.zero;
        //스크린 좌표를 recttransform기준의 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        //생명 게이지 이미지의 위치를 변경
        rectTrans.localPosition = localPos;
    }

    public void Setting(string text, Color color, Vector3 mPosition, Vector3 mOffset, Canvas renderCanvas)
    {
        //출력할 정보를 세팅해 줍니다.
        GetComponent<TextMeshProUGUI>().text = text;
        GetComponent<TextMeshProUGUI>().color = color;

        targetPosition = mPosition;
        canvas = renderCanvas;
        offset = mOffset;
    }
}
