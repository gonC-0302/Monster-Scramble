using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using DG.Tweening;

public class BaseCrystalHP : NetworkBehaviour, IDamagable
{
    [Networked]
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    public bool IsMonsterCrystal { get; set; }
    [SerializeField]
    private Image _hpGage;
    static readonly float _maxHP = 100;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority) return;
        TeamID = Runner.LocalPlayer.PlayerId;
        NetworkedHP = _maxHP;
    }
    public void UpdateHPGage()
    {
        _hpGage.DOFillAmount(NetworkedHP / _maxHP, 0.25f).SetLink(gameObject);
    }
    void HealthChanged()
    {
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        UpdateHPGage();
        EffectPool.instance.GetHitEffect(gameObject.transform.position);
        if (NetworkedHP < 1)
        {
            // 負けた場合
            if(TeamID == Runner.LocalPlayer.PlayerId) GameManager.instance.Lose();
            // 勝った場合
            else GameManager.instance.Win();
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
