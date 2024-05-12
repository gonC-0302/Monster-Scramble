using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    static readonly int ATTACK_KEY = Animator.StringToHash("Attack");
    static readonly int MOVE_KEY = Animator.StringToHash("Move");

    void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    public void PlayMoveAnimation()
    {
        if(!_anim.GetBool(MOVE_KEY))
        {
            _anim.SetBool(MOVE_KEY, true);
        }
    }
    public void StopMoveAnimation()
    {
        if (_anim.GetBool(MOVE_KEY))
        {
            _anim.SetBool(MOVE_KEY, false);
        }
    }
    public void PlayAttackAnimation()
    {
        _anim.SetTrigger(ATTACK_KEY);
    }
}
