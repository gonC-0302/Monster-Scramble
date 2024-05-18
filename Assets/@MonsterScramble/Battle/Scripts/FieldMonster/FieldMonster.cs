using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class FieldMonster : MonoBehaviour
{
    [SerializeField]
    private SearchArea _searchArea;

    private void Awake()
    {
        var _stateManager = GetComponent<BattleStateManager>();
        _searchArea.Init(_stateManager);
    }
}
