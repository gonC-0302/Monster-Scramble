using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[CreateAssetMenu(menuName = "MonsterDataSO", fileName = "MonsterData")]
public class MonsterDataSO : ScriptableObject
{
    public List<MonsterData> _monsterDatasList = new List<MonsterData>();

    [Serializable]
    public class MonsterData
    {
        public int ID;
        public int MaxHP;
        public float MaxSpeed;
        public int Power;
        public float AttackInterval;
        public string MonsterName;
        public Sprite Image;
        public int Cost;
        public int GetRate;
        public NetworkPrefabRef SummonMonsterPrefab;
    }
}
