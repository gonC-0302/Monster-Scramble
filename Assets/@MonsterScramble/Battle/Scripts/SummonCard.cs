using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MonsterScramble
{
    public class SummonCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerDownHandler
    {
        private Vector2 _prevPos;
        static readonly string FieldLayerName = "Field";
        [SerializeField] private MonsterSummoner _summoner;
        [SerializeField] private PlayerStateManager _stateManager;

        public void OnPointerDown(PointerEventData eventData)
        {
            _prevPos = transform.position;
            _stateManager.SwitchState(State.Summon);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // ドラッグ前の位置を記憶しておく
            //_prevPos = transform.position;
            //_stateManager.SwitchState(State.Summon);
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
                    _summoner.SummonMonster(0, hit.point);
                }
            }
            _stateManager.SwitchState(State.Move);
            // ドラッグ前の位置に戻す
            transform.position = _prevPos;
        }
    }
}