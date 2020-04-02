using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTimer : StateMachineBehaviour
{
    //콤보공격 등의 타임을 잽니다.(하나의)
    public string timerName;
    float time;
    
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        time += Time.deltaTime;
        animator.SetFloat(timerName, time);//시간을 바꿔준다.
        
    }

    //시간을 리셋한다.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
        animator.SetFloat(timerName, time);        
    }

}
