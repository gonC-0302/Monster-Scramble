using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterStateMachine))]
[RequireComponent(typeof(CharacterController))]
public class PlayerAttack : NetworkBehaviour
{
    private float _attackInterval;
    private int _attackPower;
    private CharacterStateMachine _stateManager;
    private float _timer;
    private IDamagable _target;
    private SummonCardSpawner _cardSpawner;

    void Awake()
    {
        _stateManager = GetComponent<CharacterStateMachine>();
    }
    public void Initialize(SummonCardSpawner cardSpawner, int attackPower = 5, float attackInterval = 1)
    {
        _attackInterval = attackInterval;
        _attackPower = attackPower;
        _cardSpawner = cardSpawner;
    }
    public override void FixedUpdateNetwork()
    {
        if (_stateManager.CurrentState == CharacterState.Death) return;
        if (!Object.HasStateAuthority) return;
        switch (_stateManager.CurrentState)
        {
            case CharacterState.PreparateAttack:
                PreparateAttack();
                break;
        }
    }
    /// <summary>
    /// 攻撃準備
    /// </summary>
    private void PreparateAttack()
    {
        _timer += Runner.DeltaTime;
        if (_timer > _attackInterval)
        {
            _timer = 0;
            // 近くに敵がいるか確認
            if (IsExistTarget())
            {
                if (_target.NetworkedHP < 1)
                {
                    _stateManager.SwitchState(CharacterState.Idle);
                    return;
                }
                else
                {
                    _stateManager.SwitchState(CharacterState.Attack);
                }
            }
        }
    }

    /// <summary>
    /// 攻撃対象が近くに存在しているか確認
    /// </summary>
    /// <returns></returns>
    private bool IsExistTarget()
    {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask(new string[] { "Character", "Crystal" });
        var offset = Vector3.up * 1f;
        Debug.DrawRay(gameObject.transform.position + offset, transform.forward * 6f, Color.blue, 1);
        if (Physics.Raycast(gameObject.transform.position + offset, transform.forward * 6f, out hit, 6f, layerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out IDamagable target))
            {
                if (target.TeamID != Runner.LocalPlayer.PlayerId)
                {
                    this._target = target;
                    return true;
                }
            }
        }
        _timer = 0;
        _stateManager.SwitchState(CharacterState.Idle);
        return false;
    }

    /// <summary>
    /// 攻撃
    /// アニメーションに登録
    /// </summary>
    public void Attack()
    {
        if (!Object.HasStateAuthority) return;
        if (_stateManager.CurrentState == CharacterState.Death) return;
        if (IsExistTarget())
        {
            if (_target.NetworkedHP < 1)
            {
                _stateManager.SwitchState(CharacterState.Idle);
                return;
            }
            AudioManager.instance.PlayAttackSE();
            var damagedHP = _target.GetDamagedHP(_attackPower);
            _target.DealDamageRpc(damagedHP);
            if (_target.IsMonsterCrystal)
            {
                if (damagedHP == 0) _cardSpawner.AddCardsList();
            }
            _stateManager.SwitchState(CharacterState.PreparateAttack);
        }
        else _stateManager.SwitchState(CharacterState.Idle);
    }
}

