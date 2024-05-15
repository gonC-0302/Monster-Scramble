using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class HitPoint : NetworkBehaviour
{
    public int PlayerID => _playerID;
    [SerializeField]
    private int _maxHP;
    [Networked, OnChangedRender(nameof(HealthChanged))]
    private float NetworkedHealth { get; set; }
    private int _playerID;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            NetworkedHealth = _maxHP;
        }
    }
    public void SetPlayerID(int playerID)
    {
        _playerID = playerID;
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
