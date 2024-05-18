using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public enum MonsterState
{
    MoveToCrystal,
    MoveToEnemy,
    PreparateAttack,
    Attack,
    Death
}

public class SummonMonster : NetworkBehaviour
{
    //[SerializeField]
    //private SearchArea _searchArea;
    private Transform _targetCrystalTran;
    private int _summonPlayerID;
    public int SummonPlayerID => _summonPlayerID;
    private Vector3 _direction;
    private float _distance;
    private Transform _targetTran;
    private NetworkCharacterController characterController;
    public MonsterState CurrentState => _currentState;
    private MonsterState _currentState;
    private CharacterAnimationManager _anim;

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        _anim = GetComponent<CharacterAnimationManager>();
        //_searchArea.Init(this);
    }

    public void Init(int playerID, Transform targetTran)
    {
        Debug.Log("init");
        _summonPlayerID = playerID;
        _targetCrystalTran = targetTran;

        _currentState = MonsterState.MoveToCrystal;
        _targetTran = _targetCrystalTran;
    }

    public void FindEnemy(Transform enemyTran)
    {
        _targetTran = enemyTran;
        SwitchState(MonsterState.MoveToEnemy);
    }

    public void SwitchState(MonsterState nextState)
    {
        _currentState = nextState;
        switch (nextState)
        {
            case MonsterState.PreparateAttack:
                _anim.StopMoveAnimation();
                break;
            case MonsterState.MoveToCrystal:
                _anim.PlayMoveAnimation();
                    break;
            case MonsterState.MoveToEnemy:
                _anim.PlayMoveAnimation();
                break;
            case MonsterState.Attack:
                _anim.PlayAttackAnimation();
                break;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            var targetPos = new Vector3(_targetTran.position.x,transform.position.y, _targetTran.position.z);
            _distance = Vector3.Distance(targetPos, transform.position);
            switch (CurrentState)
            {
                case MonsterState.MoveToCrystal:
                case MonsterState.MoveToEnemy:
                    if (_distance < 1)
                    {
                        SwitchState(MonsterState.PreparateAttack);
                        return;
                    }
                    _direction = (targetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case MonsterState.PreparateAttack:
                    transform.LookAt(targetPos);
                    break;
                case MonsterState.Attack:
                    _targetTran = transform;
                    break;
            }
        }
    }
}