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
    private HitPoint _targetHP;
    private BattleCharacterBase _targetChara;
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
    public void SetAttackTarget(HitPoint targetHP)
    {
        this._targetHP = targetHP;
        _attackInterval = 0;
    }

    public void SetAttackTarget(BattleCharacterBase targetCharacter)
    {
        this._targetChara = targetCharacter;
        _attackInterval = 0;
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
            if (!IsExitTarget()) _stateManager.SwitchState(BattleState.Idle);
            _stateManager.SwitchState(BattleState.Attack);
        }
    }
    /// <summary>
    /// 敵がまだ生きているか確認
    /// </summary>
    /// <returns></returns>
    private bool IsExitTarget()
    {
        //return _targetHP != null ? true : false;
        return _targetChara != null ? true : false;
    }
    /// <summary>
    /// 攻撃
    /// アニメーションに登録
    /// </summary>
    public void Attack()
    {
        _targetChara.GetHit(_attackPower);
        //_targetHP.DealDamageRpc(_attackPower);
        _stateManager.SwitchState(BattleState.PreparateAttack);
    }
}
