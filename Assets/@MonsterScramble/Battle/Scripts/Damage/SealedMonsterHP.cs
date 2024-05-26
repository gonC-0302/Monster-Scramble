using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SealedMonsterHP : NetworkBehaviour,IDamagable
{
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    //[SerializeField]
    //private int _maxHP;
    private int _monsterID;

    public override void Spawned()
    {
        //if (Object.HasStateAuthority)
        //{
        //    NetworkedHP = _maxHP;
        //}
    }

    public void Init(int monsterID,int teamID)
    {
        _monsterID = monsterID;
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
            attacker.GetSummonCard(_monsterID);
            NetworkedHP = 0;
            Destroy(gameObject);
        }
    }
}
