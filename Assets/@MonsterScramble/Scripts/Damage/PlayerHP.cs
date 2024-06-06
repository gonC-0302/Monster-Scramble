using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using DG.Tweening;

[RequireComponent(typeof(CharacterStateMachine))]
public class PlayerHP : NetworkBehaviour, IDamagable
{
    [Networked]
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    public bool IsMonsterCrystal { get; set; }
    static readonly float _maxHP = 30;
    [SerializeField]
    private GameObject _hpCanvas;
    [SerializeField]
    private Image _hpGage;
    private PlayerSpawner _spawner;
    private CharacterStateMachine _stateMachine;

    public void Initialize(PlayerSpawner spawner)
    {
        _spawner = spawner;
    }

    public override void Spawned()
    {
        _stateMachine = GetComponent<CharacterStateMachine>();
        if (!Object.HasStateAuthority) return;
        TeamID = Runner.LocalPlayer.PlayerId;
        NetworkedHP = _maxHP;
    }
    public void UpdateHPGage()
    {
        _hpGage.DOFillAmount(NetworkedHP / _maxHP, 0.25f).SetLink(gameObject);
    }
    private void LateUpdate()
    {
        _hpCanvas.transform.rotation = Camera.main.transform.rotation;
    }
    void HealthChanged()
    {
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        if (_stateMachine.CurrentState == CharacterState.Death) return;
        if (Object.HasStateAuthority)
        {
            AudioManager.instance.PlayDamageSE();
        }
        UpdateHPGage();
        EffectPool.instance.GetHitEffect(gameObject.transform.position);
        if (NetworkedHP < 1)
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            _stateMachine.SwitchState(CharacterState.Death);
            EffectPool.instance.GetDeathEffect(gameObject.transform.position);
            if (!Object.HasStateAuthority) return;
            StartCoroutine(_spawner.RespawnPlayer());
        }
    }
    public float GetDamagedHP(float power)
    {
        return Mathf.Clamp(NetworkedHP - power, 0, _maxHP);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damagedHP)
    {
        if (NetworkedHP <= 0) return;
        NetworkedHP = damagedHP;
    }
}
