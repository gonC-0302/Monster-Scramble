using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SummonCard : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
     private MonsterSummoner _summoner;
    private Vector2 _prevPos;
    //private BattleStateManager _stateManager;
    [SerializeField] private TextMeshProUGUI _idText;
    static readonly string FieldLayerName = "Field";
    private int _monsterID;
    

    public void Init(int monsterID, MonsterSummoner summoner)
    {
        _summoner = summoner;
        _idText.text = monsterID.ToString();
        _monsterID = monsterID;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _prevPos = transform.position;
        //_stateManager.SwitchState(BattleState.Summon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ中は位置を更新する
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
            {
                _summoner.SummonMonster(hit.point, _monsterID);
                Destroy(gameObject);
            }
        }
        //_stateManager.SwitchState(BattleState.Idle);
        // ドラッグ前の位置に戻す
        transform.position = _prevPos;
    }
}
