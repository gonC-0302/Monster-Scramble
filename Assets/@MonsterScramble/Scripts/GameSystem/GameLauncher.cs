using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class GameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner networkRunnerPrefab;
    [SerializeField]
    private NetworkPrefabRef _playerBasePrefab;
    [SerializeField]
    private Transform _p1BaseTran,_p2BaseTran;
    [SerializeField]
    private MonsterSummoner _summonner;
    [SerializeField]
    private PlayerSpawner _playerSpawner;
    [SerializeField]
    private MonsterCrystalSpawner _sealedMonsterSpawner;
    private NetworkRunner _networkRunner;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _homeCanvas;
    [SerializeField]
    private NetworkPrefabRef _timerManager;

    /// <summary>
    /// オンライン接続開始
    /// </summary>
    public async void TryMatching()
    {
        _networkRunner = Instantiate(networkRunnerPrefab);
        _networkRunner.AddCallbacks(this);
        var result = await _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SceneManager = _networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            PreparateBattle();
        }
        else Debug.Log("失敗！");
    }
   
    /// <summary>
    /// プレイヤー生成・クリスタル生成・モンスターサモナーセットアップ
    /// </summary>
    /// <param name="playerID"></param>
    private void PreparateBattle()
    {
        _playerSpawner.SpawnPlayer(_networkRunner);
        SpawnCrystal();
        _summonner.Init(_networkRunner);
    }
    private void SpawnCrystal()
    {
        var teamID = _networkRunner.LocalPlayer.PlayerId;
        var crystalSpawnPos = Vector3.zero;
        if (teamID == 1) crystalSpawnPos = _p1BaseTran.position;
        else if (teamID == 2) crystalSpawnPos = _p2BaseTran.position;
        _networkRunner.Spawn(_playerBasePrefab, crystalSpawnPos, Quaternion.identity, _networkRunner.LocalPlayer);
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.SessionInfo.PlayerCount == 2)
        {
            _homeCanvas.SetActive(false);
            GameManager.instance.StartBattle();

        // 以降マスタークライアント側だけの処理
            if (!runner.IsSharedModeMasterClient) return;
            _networkRunner.Spawn(_timerManager, Vector3.zero, Quaternion.identity, _networkRunner.LocalPlayer).GetComponent<BattleTimeManager>();
            StartCoroutine(_sealedMonsterSpawner.SpawnMonsterCrystal(_networkRunner));
        }
    }
    /// <summary>
    /// オンライン接続終了
    /// </summary>
    public void ShutDown()
    {
        _networkRunner.Shutdown();
        Destroy(_networkRunner);
    }
    public void OnObjectExitAOI(NetworkRunner runner,NetworkObject obj ,PlayerRef playerRef) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef playerRef) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner,NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player,ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key,float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}