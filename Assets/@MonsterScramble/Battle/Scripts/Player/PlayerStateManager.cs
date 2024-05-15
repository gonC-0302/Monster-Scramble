using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Move,
    MoveToEnemy,
    Summon,
    PreparateAttack,
    Attack,
    Death
}

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState CurrentState => _currentState;
    private PlayerState _currentState;
    private PlayerAnimation _playerAnim;

    private void Awake()
    {
        _playerAnim = GetComponent<PlayerAnimation>();
    }
    public void SwitchState(PlayerState nextState)
    {
        _currentState = nextState;
        switch(nextState)
        {
            case PlayerState.Idle:
            case PlayerState.PreparateAttack:
                _playerAnim.StopMoveAnimation();
                break;
            case PlayerState.Move:
            case PlayerState.MoveToEnemy:
                _playerAnim.PlayMoveAnimation();
                break;
            case PlayerState.Attack:
                _playerAnim.PlayAttackAnimation();
                break;
        }
    }
}
