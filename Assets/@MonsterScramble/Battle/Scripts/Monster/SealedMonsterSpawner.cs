using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SealedMonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkPrefabRef _fieldMonsterPrefab;
    [SerializeField]
    private List<Transform> _spawnTransList = new List<Transform>();
    private NetworkRunner _networkRunner;
    private const int _teamID = 0;

    public void SpawnFieldMonster(NetworkRunner runner)
    {
        this._networkRunner = runner;
        for (int i = 0; i < _spawnTransList.Count; i++)
        {
            var avatarSpawnPos = _spawnTransList[i].position;
            var avatarRot = _spawnTransList[i].rotation;
            var avatar = _networkRunner.Spawn(_fieldMonsterPrefab, avatarSpawnPos, avatarRot, _networkRunner.LocalPlayer);
            var monsterID = Random.Range(1, 5);
            avatar.GetComponent<SealedMonsterHP>().Init(monsterID, _teamID);
        }
    }
}

