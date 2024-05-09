using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterScramble
{
    public class MonsterSummoner : MonoBehaviour
    {
        [SerializeField] private GameObject _testMonster;

        public void SummonMonster(int monsterID,Vector3 summonPos)
        {
            Instantiate(_testMonster, summonPos, Quaternion.identity);
        }
    }
}