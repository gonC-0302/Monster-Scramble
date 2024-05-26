using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(BattleStateManager))]
public class AttackManager : NetworkBehaviour
{
    [SerializeField]
    private float _attackInterval;
    [SerializeField]
    private int _attackPower;
    private BattleStateManager _stateManager;
    private float _timer;
    private IDamagable _target;
    private SummonCardsList _summonCardsList;
    private Collider[] _targetCol = new Collider[3];
    private int _teamID;

    public void Init(SummonCardsList list,int teamID)
    {
        _summonCardsList = list;
        _teamID = teamID;
    }
    void Awake()
    {
        _stateManager = GetComponent<BattleStateManager>();
    }
    void Update()
    {
        switch (_stateManager.CurrentState)
        {
            case BattleState.PreparateAttack:
                PreparateAttack();
                break;
        }
    }
    /// <summary>
    /// 攻撃準備
    /// </summary>
    private void PreparateAttack()
    {
        _timer += Time.deltaTime;
        if (_timer > _attackInterval)
        {
            _attackInterval = 2f;
            _timer = 0;
            if (IsExistTarget())
            {
                _stateManager.SwitchState(BattleState.Attack);
            }
        }
    }
    private void SetTarget(IDamagable target)
    {
        this._target = target;
    }
    private bool IsExistTarget()
    {
        int layerMask = LayerMask.GetMask(new string[] { "Character", "Crystal" });
        var num = Physics.OverlapSphereNonAlloc(gameObject.transform.position + transform.forward * 1.5f, 3f, _targetCol, layerMask);
        if (num >= 1)
        {
            for (int i = 0; i < _targetCol.Length; i++)
            {
                if (_targetCol[i] == null) continue;
                if (_targetCol[i].gameObject.TryGetComponent<IDamagable>(out IDamagable target))
                {
                    if (target.TeamID != _teamID)
                    {
                        SetTarget(target);
                        return true;
                    }
                }
            }
        }
        _stateManager.SwitchState(BattleState.Idle);
        return false;
    }

    /// <summary>
    /// 攻撃
    /// アニメーションに登録
    /// </summary>
    public void Attack()
    {
        if (_target == null) return;
        _target.DealDamageRpc(_attackPower, this);
        _stateManager.SwitchState(BattleState.PreparateAttack);
    }
    public void GetSummonCard(int monsterID)
    {
        _summonCardsList.AddCardsList(monsterID);
    }
}
