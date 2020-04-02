using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolOperator2 : StateMachineBehaviour
{
    //스테이트 머신에서 상태에 들어가거나 나올때 설정해주는 클래스입니다.
    public string boolName;
    public bool status;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, !status);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, !status);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
	}
}
