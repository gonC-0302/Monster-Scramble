using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MonsterSummoner : MonoBehaviour
{
    [SerializeField]
    private Transform _p1BaseCrystalTran,_p2BaseCryStalTran;        // モンスターの最終目的地（敵の拠点）
    [SerializeField]
    private SummonCardSpawner _summonCardsList;
    private int _teamID;
    private NetworkRunner _runner;

    public void Init(NetworkRunner runner)
    {
        _teamID = runner.LocalPlayer.PlayerId;
        _runner = runner;
    }

    /// <summary>
    /// モンスターを召喚する
    /// </summary>
    /// <param name="summonPos"></param>
    /// <param name="monsterID"></param>
    public void SummonMonster(Vector3 summonPos,int monsterID)
    {
        var data = DataBaseManager.instance.dataSO._monsterDatasList.Find(x => x.ID == monsterID);
        var prefab = data.SummonMonsterPrefab;
        var monster = _runner.Spawn(prefab, summonPos, Quaternion.identity, _runner.LocalPlayer);
        if(_teamID == 1) monster.GetComponent<SummonMonsterMovement>().Init(_p2BaseCryStalTran,monsterID);
        else if(_teamID == 2) monster.GetComponent<SummonMonsterMovement>().Init(_p1BaseCrystalTran,monsterID);
        monster.GetComponent<SummonMonsterAttack>().Initialize(_summonCardsList, data.Power,data.AttackInterval);
        monster.GetComponent<SummonMonsterHP>().Initialize(monsterID);
        AudioManager.instance.PlaySummonSE();
    }
}