using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private HitPoint _hp;
    private void Awake()
    {
        _hp = GetComponent<HitPoint>();
    }
    public void GetHit(int damage)
    {
        _hp.DealDamageRpc(damage);
    }
}
