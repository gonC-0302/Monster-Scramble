using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MonsterSummoner : MonoBehaviour
{
    [SerializeField]
    private NetworkPrefabRef _monsterPrefab;
    [SerializeField]
    private Transform _p1CrystalTran,_p2CryStalTran;
    private int _playerID;
    private NetworkRunner _runner;
    

    public void Init(int playerID,NetworkRunner runner)
    {
        _playerID = playerID;
        _runner = runner;
    }

    public void SummonMonster(Vector3 summonPos)
    {
        var monster = _runner.Spawn(_monsterPrefab, summonPos, Quaternion.identity, _runner.LocalPlayer);

        if(_playerID == 1)
        {
            Debug.Log("summon");
            monster.GetComponent<SummonMonster>().Init(_playerID,_p2CryStalTran);
        }
        else if(_playerID == 2)
        {
            monster.GetComponent<SummonMonster>().Init(_playerID, _p1CrystalTran);
        }
    }
}