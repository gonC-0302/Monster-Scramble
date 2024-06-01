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
    [SerializeField]
    private SummonCardsList _summonCardsList;
    private NetworkRunner _networkRunner;
    private const int _characterID = 0;

    public void SpawnPlayer(int playerID, NetworkRunner runner)
    {
        this._networkRunner = runner;
        Vector3 playerSpawnPos = Vector3.zero;
        var playerRot = Quaternion.identity;
        if (playerID == 1)
        {
            playerSpawnPos = _p1AvatarTran.position;
            playerRot = _p1AvatarTran.rotation;
        }
        else if (playerID == 2)
        {
            playerSpawnPos = _p2AvatarTran.position;
            playerRot = _p2AvatarTran.rotation;
        }
        var player = _networkRunner.Spawn(playerAvatarPrefab, playerSpawnPos, playerRot, _networkRunner.LocalPlayer);
        InitPlayer(player.gameObject,playerID);
    }

    private void InitPlayer(GameObject player,int playerID)
    {
        player.GetComponent<PlayerHP>().Init(playerID,player.transform);
        player.GetComponent<PlayerMovement>().SetPlayerID(playerID);
        player.GetComponent<AttackManager>().Init(_summonCardsList,playerID);
    }
}
