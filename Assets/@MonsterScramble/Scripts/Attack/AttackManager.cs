using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(CharacterStateMachine))]
public class AttackManager : NetworkBehaviour
{
    [SerializeField]
    private float _attackInterval = 0.5f;
    [SerializeField]
    private int _attackPower;
    private CharacterStateMachine _stateManager;
    private float _timer;
    private IDamagable _target;
    private SummonCardsList _summonCardsList;
    private Collider[] _targetCol = new Collider[3];
    private int _teamID;

    void Awake()
    {
        _stateManager = GetComponent<CharacterStateMachine>();
    }
    public void Init(SummonCardsList list,int teamID)
    {
        _summonCardsList = list;
        _teamID = teamID;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.TryGetComponent(out IDamagable target))
        {
            if (target.TeamID == _teamID) return;
            transform.LookAt(hit.gameObject.transform);
            _stateManager.SwitchState(CharacterState.PreparateAttack);
        }
    }
    public override void FixedUpdateNetwork()
    {
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
        //_timer += Time.deltaTime;
        if (_timer > _attackInterval)
        {
            _timer = 0;
            // 近くに敵がいるか確認
            if (IsExistTarget())
            {
                _stateManager.SwitchState(CharacterState.Attack);
            }
        }
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
                        this._target = target;
                        return true;
                    }
                }
            }
        }
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

        if (_target.NetworkedHP < 1)
        {
            _stateManager.SwitchState(CharacterState.PreparateAttack);
            return;
        }

        var damagedHP = Mathf.Clamp(_target.NetworkedHP - _attackPower, 0, 9999);
        _target.DealDamageRpc(damagedHP);
        if (_target.IsSealedMonster)
        {
            if (damagedHP == 0) GetSummonCard();
        }
        _stateManager.SwitchState(CharacterState.PreparateAttack);
    }
    public void GetSummonCard()
    {
        var randomID = Random.Range(0, 4);
        _summonCardsList.AddCardsList(randomID);
    }
}
