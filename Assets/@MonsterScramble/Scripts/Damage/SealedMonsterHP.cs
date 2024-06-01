using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class SealedMonsterHP : NetworkBehaviour,IDamagable
{
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    [Networked]
    public bool IsSealedMonster { get; set; }
    public int MaxHP { get; set; }
    [SerializeField]
    private Image _hpGage;

    public override void Spawned()
    {
        IsSealedMonster = true;
    }

    public void Init(int teamID)
    {
        TeamID = teamID;
    }

    public void UpdateHPGage()
    {
        _hpGage.fillAmount = NetworkedHP / MaxHP;
    }

    void HealthChanged()
    {
        if (NetworkedHP < 1)
        {
            gameObject.SetActive(false);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damagedHP)
    {
        NetworkedHP = damagedHP;
    }
}
