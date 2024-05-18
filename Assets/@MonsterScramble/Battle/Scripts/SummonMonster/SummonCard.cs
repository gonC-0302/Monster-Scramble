using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SummonCard : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private MonsterSummoner _summoner;
    private Vector2 _prevPos;
    private BattleStateManager _stateManager;
    static readonly string FieldLayerName = "Field";

    public void Init(BattleStateManager stateManager)
    {
        this._stateManager = stateManager;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _prevPos = transform.position;
        _stateManager.SwitchState(BattleState.Summon);
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
                _summoner.SummonMonster(hit.point);
            }
        }
        _stateManager.SwitchState(BattleState.Idle);
        // ドラッグ前の位置に戻す
        transform.position = _prevPos;
    }
}
