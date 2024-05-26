using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCardsList : MonoBehaviour
{
    [SerializeField] private SummonCard _summonCardPrefab;
    [SerializeField] private Transform _contentTran;
    [SerializeField] private MonsterSummoner _summoner;

    public void AddCardsList(int monterID)
    {
        var card = Instantiate(_summonCardPrefab, _contentTran);
        card.Init(monterID, _summoner);
    }
}
