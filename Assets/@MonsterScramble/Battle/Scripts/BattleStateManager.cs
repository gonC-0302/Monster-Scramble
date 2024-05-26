using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public enum BattleState
{
    Idle,
    Move,
    MoveToTarget,
    //Summon,
    PreparateAttack,
    Attack,
    Death
}

public class BattleStateManager : MonoBehaviour
{
    public BattleState CurrentState => _currentState;
    private BattleState _currentState;
    private CharacterAnimationManager _animManager;

    private void Awake()
    {
        _animManager = GetComponent<CharacterAnimationManager>();
    }
    public void SwitchState(BattleState nextState)
    {
        _currentState = nextState;
        switch(nextState)
        {
            case BattleState.Idle:
            case BattleState.PreparateAttack:
            //case BattleState.Summon:
                _animManager.StopMoveAnimation();
                break;
            case BattleState.Move:
            case BattleState.MoveToTarget:
                _animManager.PlayMoveAnimation();
                break;
            case BattleState.Attack:
                _animManager.PlayAttackAnimation();
                break;
        }
    }
}
