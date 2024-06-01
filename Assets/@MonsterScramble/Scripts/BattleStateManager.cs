using System.Collections;
using System.Collections.Generic;
using Fusion;
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

public class CharacterStateMachine : MonoBehaviour
{
    public CharacterState CurrentState => _currentState;
    private CharacterState _currentState;
    private CharacterAnimationManager _animManager;

    private void Awake()
    {
        _animManager = GetComponent<CharacterAnimationManager>();
    }
    public void SwitchState(CharacterState nextState)
    {
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
        }
    }
}
