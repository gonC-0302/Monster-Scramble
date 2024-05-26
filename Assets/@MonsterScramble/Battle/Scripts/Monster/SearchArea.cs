//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SearchArea : MonoBehaviour
//{
//    [SerializeField]
//    private BattleStateManager _stateManager;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.TryGetComponent<HitPoint>(out HitPoint hp))
//        {
//            Debug.Log("Player発見");
//            _stateManager.SwitchState(BattleState.PreparateAttack);
//        }
//    }
//}
