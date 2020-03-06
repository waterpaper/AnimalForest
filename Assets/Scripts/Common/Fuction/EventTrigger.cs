using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public enum EventAccess
    {
        EventAccess_Player,
        EventAccess_Target,
        EventAccess_Not,
        EventAccess_Null
    }

    public enum EventKind
    {
        EventKind_BlueLight,
        EventKind_ChoiceTerrain
    }

    [Header("EvnetTrigger Infomation")]
    public int eventID;
    public float triggerActiveDistance =3.0f;
    public EventKind NowEvnetkind;
    public EventAccess NowAccessKind;
    public GameObject targetObject;
    public GameObject addConversationObject= null;

    private TEvent _Tevent;
    private bool IsStarted = false;
    

    public void Awake()
    {
        if(NowEvnetkind == EventKind.EventKind_BlueLight)
        {
            _Tevent = new Event_BlusLight();
        }
        else if(NowEvnetkind == EventKind.EventKind_ChoiceTerrain)
        {
            _Tevent = new Event_ChoiceTerrain();
        }

        if (NowAccessKind == EventAccess.EventAccess_Player)
        {
            targetObject = PlayerManager.instance.gameObject;
        }


        if (PlayerManager.instance.IsClearEventList(eventID))
            Destroy(this.gameObject);
    }

    public void LateUpdate()
    {
        if(NowAccessKind != EventAccess.EventAccess_Not && !IsStarted)
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
        if (NowAccessKind == EventAccess.EventAccess_Player || NowAccessKind == EventAccess.EventAccess_Target)
        {
            //거리를 계산해 근접할 경우만 true를 반환합니다.
            Vector3 offset = targetObject.transform.position - transform.position;

            if (Vector3.SqrMagnitude(offset) < triggerActiveDistance * triggerActiveDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (NowAccessKind == EventAccess.EventAccess_Not)
        {
            return true;
        }

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
