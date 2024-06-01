using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MonsterSummoner : MonoBehaviour
{
    [SerializeField]
    private Transform _p1CrystalTran,_p2CryStalTran;        // モンスターの最終目的地（敵の拠点）
    [SerializeField]
    private SummonCardsList _summonCardsList;
    private int _teamID;
    private NetworkRunner _runner;
    [SerializeField]
    private Material _teamMarker1, _teamMarker2;

    public void Init(int teamID,NetworkRunner runner)
    {
        _teamID = teamID;
        _runner = runner;
    }

    /// <summary>
    /// モンスターを召喚する
    /// </summary>
    /// <param name="summonPos"></param>
    /// <param name="monsterID"></param>
    public void SummonMonster(Vector3 summonPos,int monsterID)
    {
        var prefab = DataBaseManager.instance.dataSO._monsterDatasList.Find(x => x.ID == monsterID).SummonMonsterPrefab;
        var monster = _runner.Spawn(prefab, summonPos, Quaternion.identity, _runner.LocalPlayer);

        if(_teamID == 1)
        {
            monster.GetComponent<SummonMonsterMovement>().Init(_teamID,_p2CryStalTran, _teamMarker1);
        }
        else if(_teamID == 2)
        {
            monster.GetComponent<SummonMonsterMovement>().Init(_teamID, _p1CrystalTran, _teamMarker2);
        }
        monster.GetComponent<AttackManager>().Init(_summonCardsList,_teamID);
        monster.GetComponent<SummonMonsterHP>().Init(_teamID);
    }
}