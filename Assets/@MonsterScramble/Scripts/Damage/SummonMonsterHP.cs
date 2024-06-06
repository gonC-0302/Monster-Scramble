using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Fusion;

[RequireComponent(typeof(CharacterStateMachine))]
[RequireComponent(typeof(NetworkObject))]
public class SummonMonsterHP : NetworkBehaviour, IDamagable
{
    [Networked]
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    [Networked]
    public float MaxHP { get; set; }
    public bool IsMonsterCrystal { get; set; }
    [SerializeField]
    private GameObject _hpCanvas;
    [SerializeField]
    private Image _hpGage;
    private CharacterStateMachine _stateMachine;

    public override void Spawned()
    {
        _stateMachine = GetComponent<CharacterStateMachine>();
        if (!Object.HasStateAuthority) return;
        TeamID = Runner.LocalPlayer.PlayerId;
    }
    public void Initialize(int monserID)
    {
        MaxHP = DataBaseManager.instance.dataSO._monsterDatasList.Find(x => x.ID == monserID).MaxHP;
        NetworkedHP = MaxHP;
    }
    private void LateUpdate()
    {
        _hpCanvas.transform.rotation = Camera.main.transform.rotation;
    }
    void HealthChanged()
    {
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        if (_stateMachine.CurrentState == CharacterState.Death) return;
        UpdateHPGage();
        EffectPool.instance.GetHitEffect(gameObject.transform.position);
        if (NetworkedHP < 1)
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            _stateMachine.SwitchState(CharacterState.Death);
            EffectPool.instance.GetDeathEffect(gameObject.transform.position);
            Destroy(gameObject, 0.1f);
        }
    }
    public void UpdateHPGage()
    {
        _hpGage.DOFillAmount(NetworkedHP / MaxHP, 0.25f).SetLink(gameObject);
    }
    public float GetDamagedHP(float power)
    {
        return Mathf.Clamp(NetworkedHP - power, 0, MaxHP);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damagedHP)
    {
        if (NetworkedHP <= 0) return;
        NetworkedHP = damagedHP;
    }
}
