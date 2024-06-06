using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public enum CharacterState
{
    Idle,
    Move,
    MoveToTarget,
    PreparateAttack,
    Attack,
    Death
}

[RequireComponent(typeof(Animator))]
public class CharacterStateMachine : MonoBehaviour
{
    public CharacterState CurrentState => _currentState;
    private CharacterState _currentState;
    private CharacterAnimationMachine _animManager;

    private void Awake()
    {
        var anim = GetComponent<Animator>();
        _animManager = new CharacterAnimationMachine(anim);
    }
    public void SwitchState(CharacterState nextState)
    {
        if (CurrentState == CharacterState.Death) return;
        _currentState = nextState;
        switch(nextState)
        {
            case CharacterState.Idle:
            case CharacterState.PreparateAttack:
                _animManager.StopMoveAnimation();
                break;
            case CharacterState.Move:
            case CharacterState.MoveToTarget:
                _animManager.PlayMoveAnimation();
                break;
            case CharacterState.Attack:
                _animManager.PlayAttackAnimation();
                break;
            case CharacterState.Death:
                _animManager.PlayDeathAnimation();
                break;
        }
    }
}
