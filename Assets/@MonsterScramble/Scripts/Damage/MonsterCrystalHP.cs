using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using DG.Tweening;

public class MonsterCrystalHP : NetworkBehaviour,IDamagable
{
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    public int TeamID { get; set; }
    public bool IsMonsterCrystal { get; set; }
    static readonly float _maxHP = 15;
    [SerializeField]
    private Image _hpGage;

    public override void Spawned()
    {
        TeamID = 0;
        IsMonsterCrystal = true;
        if (!Object.HasStateAuthority) return;
        NetworkedHP = _maxHP;
    }
    public void UpdateHPGage()
    {
        _hpGage.DOFillAmount(NetworkedHP / _maxHP,0.25f).SetLink(gameObject);
    }
    void HealthChanged()
    {
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        UpdateHPGage();
        if (NetworkedHP == _maxHP) return;
        EffectPool.instance.GetHitEffect(gameObject.transform.position);
        if (NetworkedHP < 1)
        {
            gameObject.SetActive(false);
            EffectPool.instance.GetDeathEffect(gameObject.transform.position);
            Destroy(gameObject, 3f);
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
