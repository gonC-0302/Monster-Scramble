using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SummonCard : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private Image _image;
    private MonsterSummoner _summoner;
    private Vector2 _prevPos;
    private int _monsterID;
    static readonly string FieldLayerName = "Field";

    /// <summary>
    /// 召喚カード初期設定
    /// </summary>
    /// <param name="monsterID"></param>
    /// <param name="summoner"></param>
    public void Init(int monsterID, MonsterSummoner summoner)
    {
        var monsterData = DataBaseManager.instance.dataSO._monsterDatasList.Find(x => x.ID == monsterID);
        _summoner = summoner;
        _costText.text = monsterData.Cost.ToString();
        _monsterID = monsterID;
        var spr = monsterData.Image;
        _image.sprite = spr;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _prevPos = transform.position;
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
        // ドラッグ前の位置に戻す
        transform.position = _prevPos;
    }
}
