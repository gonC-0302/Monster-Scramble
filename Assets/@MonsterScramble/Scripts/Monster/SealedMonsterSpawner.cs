using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SealedMonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkPrefabRef SealedMonsterPrefab;

    [SerializeField]
    private float _spawnRadius; // 封印モンスターの出現範囲
    private NetworkRunner _networkRunner;
    private const int _teamID = 0;  // 0:無所属

    /// <summary>
    /// 封印されたモンスターをスポーン（ランダムなタイミング・ランダムな場所）
    /// </summary>
    /// <param name="runner"></param>
    /// <returns></returns>
    public IEnumerator SpawnSealedMonster(NetworkRunner runner)
    {
        while(GameManager.instance.CurrentGameState == GameState.Battle)
        {
            this._networkRunner = runner;
            var randomPos = _spawnRadius * Random.insideUnitCircle;
            var avatarSpawnPos = new Vector3(randomPos.x, 0, randomPos.y);
            //var monsterID = Random.Range(0, 4);
            //var prefab = DataBaseManager.instance.dataSO._monsterDatasList.Find(x => x.ID == monsterID).SealedMonsterPrefab;
            var sealedMonster = _networkRunner.Spawn(SealedMonsterPrefab, avatarSpawnPos, Quaternion.identity, _networkRunner.LocalPlayer);
            sealedMonster.GetComponent<SealedMonsterHP>().Init(_teamID);
            var randomTime = Random.Range(10, 20);
            yield return new WaitForSeconds(randomTime);
        }
    }
}

