using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab;
    [SerializeField]
    private Transform _p1AvatarTran, _p2AvatarTran;
    [SerializeField]
    private SummonCard _summonCard;
    private NetworkRunner _networkRunner;

    public void SpawnPlayer(int playerID, NetworkRunner runner)
    {
        this._networkRunner = runner;
        Vector3 avatarSpawnPos = Vector3.zero;
        var avatarRot = Quaternion.identity;
        if (playerID == 1)
        {
            avatarSpawnPos = _p1AvatarTran.position;
            avatarRot = _p1AvatarTran.rotation;
        }
        else if (playerID == 2)
        {
            avatarSpawnPos = _p2AvatarTran.position;
            avatarRot = _p2AvatarTran.rotation;
        }
        var avatar = _networkRunner.Spawn(playerAvatarPrefab, avatarSpawnPos, avatarRot, _networkRunner.LocalPlayer);
        avatar.GetComponent<BattleCharacterBase>().SetPlayerID(playerID);
        var stateManager = avatar.GetComponent<BattleStateManager>();
        _summonCard.Init(stateManager);
    }
}
