using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHpBar : MonoBehaviour
{
    //canvas를 렌더링 하는 카메라
    private Camera uiCamera;
    //UI용 최상위 캔버스
    private Canvas canvas;
    //부모 rectTransform 컴포넌트
    private RectTransform rectParent;
    //자신 RectTransform 컴포넌트 
    private RectTransform rectHp;
    //enemy 이름을 나타내는 컴포넌트
    private TextMeshProUGUI _enemyNameTextUI;
    //enemy 레벨을 나타내는 컴포넌트
    private TextMeshProUGUI _enemyLevelTextUI;

    //Hpbar 이미지의 위치를 조절할 오프셋
    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    private void Awake()
    {
        _enemyNameTextUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _enemyLevelTextUI = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        //월드 좌표를 스크린 좌표로 변환
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

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
        rectHp.localPosition = localPos;
    }

    public void EnemyInfoSetting(string name, int level)
    {
        _enemyNameTextUI.text = name;
        _enemyLevelTextUI.text = level.ToString();
    }
}
