//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Fusion;

//public class SealedMonsterHitPoint : HitPoint
//{
//    private int _monsterID;
//    public void SetMonsterID(int id)
//    {
//        _monsterID = id;
//    }

//    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
//    public void DealDamageRpc(float damage, AttackManager attaker)
//    {
//        _networkedHealth -= damage;
//        if (_networkedHealth < 1)
//        {
//            attaker.GetSummonCard(_monsterID);
//            _networkedHealth = 0;
//            Destroy(gameObject);
//        }
//    }
//}
