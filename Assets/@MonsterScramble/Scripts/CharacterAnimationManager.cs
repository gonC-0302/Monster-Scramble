using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationManager : MonoBehaviour
{
    private Animator _anim;
    static readonly int ATTACK_KEY = Animator.StringToHash("Attack");
    static readonly int MOVE_KEY = Animator.StringToHash("Move");

    void Awake()
    {
        _anim = GetComponent<Animator>();
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
        _anim.SetTrigger(ATTACK_KEY);
    }
}
