using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkPrefabRef _p1Prefab,_p2Prefab;
    [SerializeField]
    private Transform _p1SpawnTran, _p2SpawnTran;
    [SerializeField]
    private SummonCardSpawner _summonCardsList;
    [SerializeField]
    private CameraMovement _camera;
    private NetworkObject player;
    private NetworkRunner _runner;

    /// <summary>
    /// 最初のプレイヤー生成
    /// </summary>
    /// <param name="runner"></param>
    public void SpawnPlayer(NetworkRunner runner)
    {
        _runner = runner;
        if (runner.LocalPlayer.PlayerId == 1)
        {
            var playerSpawnPos = _p1SpawnTran.position;
            var playerRot = _p1SpawnTran.rotation;
            player = runner.Spawn(_p1Prefab, playerSpawnPos, playerRot, runner.LocalPlayer);
            player.GetComponent<PlayerAttack>().Initialize(_summonCardsList);
            player.GetComponent<PlayerHP>().Initialize(this);
            _camera.SetCameraTarget(player.gameObject.transform);
        }
        else if (runner.LocalPlayer.PlayerId == 2)
        {
            var playerSpawnPos = _p2SpawnTran.position;
            var playerRot = _p2SpawnTran.rotation;
            player = runner.Spawn(_p2Prefab, playerSpawnPos, playerRot, runner.LocalPlayer);
            player.GetComponent<PlayerAttack>().Initialize(_summonCardsList);
            player.GetComponent<PlayerHP>().Initialize(this);
            _camera.SetCameraTarget(player.gameObject.transform);
        }
    }

    /// <summary>
    /// プレイヤーをリスポーン
    /// </summary>
    /// <returns></returns>
    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(5f);
        _runner.Despawn(player);
        if (_runner.LocalPlayer.PlayerId == 1)
        {
            var playerSpawnPos = _p1SpawnTran.position;
            var playerRot = _p1SpawnTran.rotation;
            player = _runner.Spawn(_p1Prefab, playerSpawnPos, playerRot, _runner.LocalPlayer);
            player.GetComponent<PlayerAttack>().Initialize(_summonCardsList);
            player.GetComponent<PlayerHP>().Initialize(this);
            _camera.SetCameraTarget(player.gameObject.transform);
        }
        else if (_runner.LocalPlayer.PlayerId == 2)
        {
            var playerSpawnPos = _p2SpawnTran.position;
            var playerRot = _p2SpawnTran.rotation;
            player = _runner.Spawn(_p2Prefab, playerSpawnPos, playerRot, _runner.LocalPlayer);
            player.GetComponent<PlayerAttack>().Initialize(_summonCardsList);
            player.GetComponent<PlayerHP>().Initialize(this);
            _camera.SetCameraTarget(player.gameObject.transform);
        }
    }
}
