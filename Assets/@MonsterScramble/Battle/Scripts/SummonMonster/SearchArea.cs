using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchArea : MonoBehaviour
{
    private BattleStateManager _stateManager;

    public void Init(BattleStateManager stateManager)
    {
        this._stateManager = stateManager;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<HitPoint>(out HitPoint hp))
        {
            _stateManager.SwitchState(BattleState.PreparateAttack);
        }
    }
}
