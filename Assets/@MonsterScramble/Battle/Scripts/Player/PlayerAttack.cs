using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(PlayerStateManager))]
public class PlayerAttack : NetworkBehaviour
{
    [SerializeField]
    private float _attackInterval;
    [SerializeField]
    private int _attackPower;
    private PlayerStateManager _stateManager;
    private float _timer;
    private HitPoint _targetHP;

    void Awake()
    {
        _stateManager = GetComponent<PlayerStateManager>();
    }
    void Update()
    {
        switch (_stateManager.CurrentState)
        {
            case PlayerState.PreparateAttack:
                PreparateAttack();
                break;
        }
    }
    public void SetAttackTarget(HitPoint targetHP)
    {
        this._targetHP = targetHP;
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
            if (!IsExitTarget()) _stateManager.SwitchState(PlayerState.Idle);
            _stateManager.SwitchState(PlayerState.Attack);
        }
    }
    /// <summary>
    /// 敵がまだ生きているか確認
    /// </summary>
    /// <returns></returns>
    private bool IsExitTarget()
    {
        return _targetHP != null ? true : false;
    }
    /// <summary>
    /// 攻撃
    /// アニメーションに登録
    /// </summary>
    public void Attack()
    {
        _targetHP.DealDamageRpc(_attackPower);
        _stateManager.SwitchState(PlayerState.PreparateAttack);
    }
}
