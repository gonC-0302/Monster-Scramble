using Fusion;
using UnityEngine;

public class GameLauncher : MonoBehaviour
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
    private FieldMonsterSpawner _fieldMonsterSpawner;
    private NetworkRunner _networkRunner;

    private async void Start()
    {
        _networkRunner = Instantiate(networkRunnerPrefab);
        var result = await _networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SceneManager = _networkRunner.GetComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            InitBattle(_networkRunner.LocalPlayer.PlayerId);
            _summonner.Init(_networkRunner.LocalPlayer.PlayerId,_networkRunner);
            if(_networkRunner.LocalPlayer.PlayerId == 1) _fieldMonsterSpawner.SpawnFieldMonster( _networkRunner);
        }
        else Debug.Log("失敗！");
    }

    private void InitBattle(int playerID)
    {
        _playerSpawner.SpawnPlayer(playerID,_networkRunner);
        SpawnCrystal(playerID);
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
        var crystal = _networkRunner.Spawn(_playerBasePrefab, towerSpawnPos, Quaternion.identity, _networkRunner.LocalPlayer);
        crystal.GetComponent<Crystal>().SetPlayerID(playerID);
    }
}