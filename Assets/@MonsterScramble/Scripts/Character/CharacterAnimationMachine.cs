using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationMachine
{
    private Animator _anim;
    static readonly int IDLE_KEY = Animator.StringToHash("Idle");
    static readonly int ATTACK_KEY = Animator.StringToHash("Attack");
    static readonly int MOVE_KEY = Animator.StringToHash("Move");
    static readonly int DEATH_KEY = Animator.StringToHash("Death");

    public CharacterAnimationMachine(Animator anim)
    {
        _anim = anim;
    }
    /// <summary>
    /// 移動アニメーション開始
    /// </summary>
    public void PlayMoveAnimation()
    {
        if(!_anim.GetBool(MOVE_KEY))
        {
            _anim.SetBool(MOVE_KEY, true);
        }
    }
    /// <summary>
    /// 移動アニメーション停止
    /// </summary>
    public void StopMoveAnimation()
    {
        if (_anim.GetBool(MOVE_KEY))
        {
            _anim.SetBool(MOVE_KEY, false);
        }
    }
    /// <summary>
    /// 攻撃アニメーション
    /// </summary>
    public void PlayAttackAnimation()
    {
        StopMoveAnimation();
        _anim.SetTrigger(ATTACK_KEY);
    }
    public void PlayDeathAnimation()
    {
        _anim.Play(IDLE_KEY);
        _anim.SetTrigger(DEATH_KEY);
    }
}
