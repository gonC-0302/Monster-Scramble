using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCardSpawner : MonoBehaviour
{
    [SerializeField] private SummonCard _summonCardPrefab;
    [SerializeField] private Transform _contentTran;
    [SerializeField] private MonsterSummoner _summoner;
    [SerializeField] private SummonGage _summonGage;

    public void AddCardsList()
    {
        var data = DataBaseManager.instance.dataSO._monsterDatasList;
        var random = Random.Range(0, 100);
        int randomID;
        if (random < data[0].GetRate)
        {
            randomID = 0;
        }
        else if(random < data[0].GetRate+ data[1].GetRate)
        {
            randomID = 1;
        }
        else if(random < data[0].GetRate + data[1].GetRate + data[2].GetRate)
        {
            randomID = 2;
        }
        else
        {
            randomID = 3;
        }
        var card = Instantiate(_summonCardPrefab, _contentTran);
        card.Init(randomID, _summoner, _summonGage);
        AudioManager.instance.PlayGetSummonCardSE();
    }
}
