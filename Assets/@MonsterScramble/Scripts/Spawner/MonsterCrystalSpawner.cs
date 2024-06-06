using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class MonsterCrystalSpawner : MonoBehaviour
{
    [SerializeField]
    private NetworkPrefabRef _monsterCrystalPrefab;
    [SerializeField]
    private float _spawnRadius; // 封印モンスターの出現範囲
    [SerializeField]
    private SummonCardSpawner _cardSpawner;

    /// <summary>
    /// 封印されたモンスターをスポーン（ランダムなタイミング・ランダムな場所）
    /// </summary>
    /// <param name="runner"></param>
    /// <returns></returns>
    public IEnumerator SpawnMonsterCrystal(NetworkRunner runner)
    {
        while(GameManager.instance.CurrentGameState == GameState.Battle)
        {
            var randomPos = _spawnRadius * Random.insideUnitCircle;
            var spawnPos = new Vector3(randomPos.x, 0, randomPos.y);
            runner.Spawn(_monsterCrystalPrefab, spawnPos, Quaternion.identity, runner.LocalPlayer);
            var randomTime = Random.Range(10f, 20f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}

