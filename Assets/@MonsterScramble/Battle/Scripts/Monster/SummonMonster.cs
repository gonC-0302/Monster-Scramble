using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SummonMonster : NetworkBehaviour
{
    private Transform _targetCrystalTran;
    private int _summonPlayerID;
    public int SummonPlayerID => _summonPlayerID;
    private Vector3 _direction;
    private float _distance;
    private Transform _targetTran;
    private NetworkCharacterController characterController;
    public BattleState CurrentState => _currentState;
    private BattleState _currentState;
    private CharacterAnimationManager _anim;
    private AttackManager attackManager;
    private Collider[] searchCol = new Collider[5];
    private bool _isBattle;
    private BattleStateManager _stateManager;

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        attackManager = GetComponent<AttackManager>();
        _anim = GetComponent<CharacterAnimationManager>();
        _stateManager = GetComponent<BattleStateManager>();
    }

    public void Init(int playerID, Transform targetTran)
    {
        _summonPlayerID = playerID;
        _targetCrystalTran = targetTran;
        _currentState = BattleState.MoveToTarget;
        _targetTran = _targetCrystalTran;
        StartCoroutine(SearchBattleTarget());
    }

    private IEnumerator SearchBattleTarget()
    {
        _isBattle = false;
        while (!_isBattle)
        {
            int layerMask = LayerMask.GetMask(new string[] { "Character" , "Crystal" });
            var num = Physics.OverlapSphereNonAlloc(gameObject.transform.position, 3f, searchCol, layerMask);
            if (num >= 1)
            {
                for (int i = 0; i < searchCol.Length; i++)
                {
                    if (searchCol[i] == null) continue;
                    if (searchCol[i].gameObject.TryGetComponent<IDamagable>(out IDamagable target))
                    {
                        if (target.TeamID == _summonPlayerID) continue;
                        _isBattle = true;
                        //attackManager.SetTargetHP(target);
                        _targetTran = searchCol[i].gameObject.transform;
                    }
                }
            }
            yield return new WaitForSeconds(3f);
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
                case BattleState.Idle:
                        _stateManager.SwitchState(BattleState.MoveToTarget);
                    break;
                case BattleState.Move:
                    _stateManager.SwitchState(BattleState.MoveToTarget);
                    break;
                case BattleState.MoveToTarget:
                    if (_distance < 5)
                    {
                        _stateManager.SwitchState(BattleState.PreparateAttack);
                        return;
                    }
                    _direction = (targetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case BattleState.PreparateAttack:
                    //if(!attackManager.IsExitTarget())
                    //{
                    //    _targetTran = _targetCrystalTran;
                    //    _stateManager.SwitchState(BattleState.Move);
                    //    StartCoroutine(SearchBattleTarget());
                    //    return;
                    //}
                    //transform.LookAt(targetPos);
                    break;
                case BattleState.Attack:
                    _targetTran = transform;
                    break;
            }
        }
    }
}