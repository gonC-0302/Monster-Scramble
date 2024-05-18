using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class HitPoint : NetworkBehaviour
{
    [SerializeField]
    private int _maxHP;
    [Networked, OnChangedRender(nameof(HealthChanged))]
    private float NetworkedHealth { get; set; }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            NetworkedHealth = _maxHP;
        }
    }
    void HealthChanged()
    {
        Debug.Log($"Health changed to: {NetworkedHealth}");
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage)
    {
        NetworkedHealth -= damage;
        if (NetworkedHealth < 1)
        {
            NetworkedHealth = 0;
            Destroy(gameObject);
        }
    }
}
