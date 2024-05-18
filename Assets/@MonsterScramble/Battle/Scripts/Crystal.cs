using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private HitPoint _hp;
    public int PlayerID => _playerID;
    protected int _playerID;

    private void Awake()
    {
        _hp = GetComponent<HitPoint>();
    }
    public void SetPlayerID(int playerID)
    {
        _playerID = playerID;
    }
    public void GetHit(int damage)
    {
        _hp.DealDamageRpc(damage);
    }
}
