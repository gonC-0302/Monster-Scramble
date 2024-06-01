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
    private SealedMonsterSpawner _sealedMonsterSpawner;
    private NetworkRunner _networkRunner;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _homeCanvas;
    [SerializeField]
    private NetworkPrefabRef _timerManager;
    
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
            var playerID = _networkRunner.LocalPlayer.PlayerId;
            PreparateBattle(playerID);
        }
        else Debug.Log("失敗！");
    }

    /// <summary>
    /// プレイヤー生成・クリスタル生成・モンスターサモナーセットアップ
    /// </summary>
    /// <param name="playerID"></param>
    private void PreparateBattle(int playerID)
    {
        _playerSpawner.SpawnPlayer(playerID, _networkRunner);
        SpawnCrystal(playerID);
        _summonner.Init(playerID, _networkRunner);
    }
    private void SpawnCrystal(int playerID)
    {
        var towerSpawnPos = Vector3.zero;
        if (playerID == 1)
        {
            towerSpawnPos = _p1BaseTran.position;
        }
        else if (playerID == 2)
        {
            towerSpawnPos = _p2BaseTran.position;
        }
        Debug.Log(playerID + "のクリスタルをスポーン");
        var crystal = _networkRunner.Spawn(_playerBasePrefab, towerSpawnPos, Quaternion.identity, _networkRunner.LocalPlayer);
        crystal.GetComponent<CrystalHP>().Init(playerID);
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //if (runner.SessionInfo.PlayerCount == 2)
        //{
            _homeCanvas.SetActive(false);
            GameManager.instance.StartBattle();

        // 以降マスタークライアント側だけの処理
            if (!runner.IsSharedModeMasterClient) return;
            var timer = _networkRunner.Spawn(_timerManager, Vector3.zero, Quaternion.identity, _networkRunner.LocalPlayer).GetComponent<BattleTimeManager>();
            //timer.Init(_networkRunner);
            StartCoroutine(_sealedMonsterSpawner.SpawnSealedMonster(_networkRunner));
        //}
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