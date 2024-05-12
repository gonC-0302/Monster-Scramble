using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public State CurrentState => _currentState;
    private State _currentState;
    private PlayerAnimation _playerAnim;

    private void Awake()
    {
        _playerAnim = GetComponent<PlayerAnimation>();
    }
    public void SwitchState(State nextState)
    {
        _currentState = nextState;
        switch(nextState)
        {
            case State.Idle:
            case State.PreparateAttack:
                _playerAnim.StopMoveAnimation();
                break;
            case State.Move:
            case State.MoveToEnemy:
                _playerAnim.PlayMoveAnimation();
                break;
            case State.Attack:
                _playerAnim.PlayAttackAnimation();
                break;
        }
    }
}
