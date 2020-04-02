using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    //해당 이벤트를 세팅해 실행하게 하는 클래스입니다.
    [Header("EvnetTrigger Infomation")]
    public int eventID;
    public float triggerActiveDistance = 3.0f;

    public EventKind nowEvnetkind;
    public EventAccess nowAccessKind;
    public GameObject targetObject;
    public GameObject addConversationObject;

    private TEvent _Tevent;
    private bool IsStarted = false;
    

    public void Awake()
    {
        //이벤트 종류에 맞는 하위클래스를 생성합니다.
        if(nowEvnetkind == EventKind.EventKind_BlueLight)
            _Tevent = new Event_BlusLight();
        else if(nowEvnetkind == EventKind.EventKind_ChoiceTerrain)
            _Tevent = new Event_ChoiceTerrain();

        if (nowAccessKind == EventAccess.EventAccess_Player)
            targetObject = PlayerManager.instance.gameObject;

        if (PlayerManager.instance.IsClearEventList(eventID))
            Destroy(this.gameObject);
    }

    public void LateUpdate()
    {
        if(nowAccessKind != EventAccess.EventAccess_Not && !IsStarted)
        {
            EventStart();
        }
    }

    public void EventStart()
    {
        if (IsStarted == true && PlayerManager.instance.IsClearEventList(eventID)) return;

        if (!IsAccessKind_Check()) return;
        
        PlayerManager.instance.AddClearEventList(eventID);
        _Tevent.EventStart();

        if (addConversationObject != null)
            addConversationObject.SetActive(true);

        IsStarted = true;
    }

    private bool IsAccessKind_Check()
    {
        if (nowAccessKind == EventAccess.EventAccess_Player || nowAccessKind == EventAccess.EventAccess_Target)
        {
            //거리를 계산해 근접할 경우만 true를 반환합니다.
            Vector3 offset = targetObject.transform.position - transform.position;

            if (Vector3.SqrMagnitude(offset) < triggerActiveDistance * triggerActiveDistance)
                return true;
        }
        else if (nowAccessKind == EventAccess.EventAccess_Not)
            return true;

        return false;
    }
}

abstract public class TEvent
{
    public GameObject EventObject;

    abstract public void EventStart();
}


public class Event_ChoiceTerrain : TEvent
{
    public Event_ChoiceTerrain()
    {
        EventObject = null;
        EventObject = GameObject.FindGameObjectWithTag("ChoiceTerrain");
    }

    public override void EventStart()
    {
        if (EventObject != null)
            EventObject.SetActive(true);
    }
}

public class Event_BlusLight : TEvent
{
    public Event_BlusLight()
    {
        EventObject = null;
        EventObject = GameObject.FindGameObjectWithTag("BlueLight");
        EventObject.SetActive(false);
    }

    public override void EventStart()
    {
        if(EventObject != null)
            EventObject.SetActive(true);
    }
}
