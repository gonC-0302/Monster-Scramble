using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SummonMonsterHP : NetworkBehaviour, IDamagable
{
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }

    public void Init(int teamID)
    {
        TeamID = teamID;
    }

    void HealthChanged()
    {
        Debug.Log($"Health changed to: {NetworkedHP}");
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage, AttackManager attacker)
    {
        if (NetworkedHP < 1) return;
        NetworkedHP -= damage;
        if (NetworkedHP < 1)
        {
            NetworkedHP = 0;
            Destroy(gameObject);
        }
    }
}
