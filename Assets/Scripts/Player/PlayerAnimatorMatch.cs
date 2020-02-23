using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorMatch : MonoBehaviour
{
    Animator _anim;
    PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void OnAnimatorMove()   //It updates every frame when animator's animations in play.
    {
        if (_playerController.IsMove)
            return;

        if (!_playerController.IsGround)
            return;

        _playerController._rigid.drag = 0;
        float multiplier = 3f;

        Vector3 dPosition = _anim.deltaPosition;   //storing delta positin of active model's position.         

        dPosition.y = 0f;   //flatten the Y (height) value of root animations.

        Vector3 vPosition = (dPosition * multiplier) / Time.fixedDeltaTime;     //defines the vector 3 value for the velocity.      

        _playerController._rigid.velocity = vPosition; //This will move the root gameObject for matching active model's position.

    }
}
