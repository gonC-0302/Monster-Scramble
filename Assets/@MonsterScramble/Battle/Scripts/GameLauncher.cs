using Fusion;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;
    [SerializeField]
    private NetworkPrefabRef playerAvatarPrefab,_playerBasePrefab;
    [SerializeField]
    private Transform _p1AvatarTran, _p2AvatarTran,_p1BaseTran,_p2BaseTran;
    private NetworkRunner networkRunner;

    private async void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        var result = await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok) InitBattle(networkRunner.LocalPlayer.PlayerId);
        else Debug.Log("失敗！");
    }

    private void InitBattle(int playerID)
    {
        Vector3 avatarSpawnPos = Vector3.zero;
        var avatarRot = Quaternion.identity;
        var towerSpawnPos = Vector3.zero;

        if (playerID == 1)
        {
            avatarSpawnPos = _p1AvatarTran.position;
            avatarRot = _p1AvatarTran.rotation;
            towerSpawnPos = _p1BaseTran.position;
        }
        else if (playerID == 2)
        {
            avatarSpawnPos = _p2AvatarTran.position;
            avatarRot = _p2AvatarTran.rotation;
            towerSpawnPos = _p2BaseTran.position;
        }

        var avatar = networkRunner.Spawn(playerAvatarPrefab, avatarSpawnPos, avatarRot, networkRunner.LocalPlayer);
        avatar.GetComponent<PlayerMovement>().SetPlayerID(playerID);
        avatar.GetComponent<HitPoint>().SetPlayerID(playerID);
        var tower = networkRunner.Spawn(_playerBasePrefab, towerSpawnPos, Quaternion.identity, networkRunner.LocalPlayer);
        tower.GetComponent<HitPoint>().SetPlayerID(playerID);
    }
}