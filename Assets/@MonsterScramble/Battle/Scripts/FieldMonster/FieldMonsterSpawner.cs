using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class FieldMonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkPrefabRef _fieldMonsterPrefab;
    [SerializeField]
    private Transform _spawnTran;
    private NetworkRunner _networkRunner;

    public void SpawnFieldMonster(NetworkRunner runner)
    {
        this._networkRunner = runner;
        var avatarRot = Quaternion.identity;
        var avatarSpawnPos = _spawnTran.position;
        avatarRot = _spawnTran.rotation;
        var avatar = _networkRunner.Spawn(_fieldMonsterPrefab, avatarSpawnPos, avatarRot, _networkRunner.LocalPlayer);
        avatar.GetComponent<BattleCharacterBase>().SetPlayerID(0);
        var stateManager = avatar.GetComponent<BattleStateManager>();
    }
}

