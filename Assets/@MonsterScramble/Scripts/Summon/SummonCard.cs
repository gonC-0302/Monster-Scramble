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
    private SummonGage _summonGage;
    private int _cost;
    static readonly string FieldLayerName = "Field";

    /// <summary>
    /// 召喚カード初期設定
    /// </summary>
    /// <param name="monsterID"></param>
    /// <param name="summoner"></param>
    public void Init(int monsterID, MonsterSummoner summoner,SummonGage summonGage)
    {
        this._summoner = summoner;
        this._summonGage = summonGage;
        SetMonsterInfo(monsterID);
    }
    /// <summary>
    /// モンスター情報をセット
    /// </summary>
    /// <param name="monsterID"></param>
    private void SetMonsterInfo(int monsterID)
    {
        var monsterData = DataBaseManager.instance.dataSO._monsterDatasList.Find(x => x.ID == monsterID);
        _costText.text = monsterData.Cost.ToString();
        var spr = monsterData.Image;
        _image.sprite = spr;
        _cost = monsterData.Cost;
        _monsterID = monsterID;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_summonGage.GetChargedSP() < _cost) return;
        _prevPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_summonGage.GetChargedSP() < _cost) return;
        // ドラッグ中は位置を更新する
        transform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_summonGage.GetChargedSP() < _cost) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
            {
                _summoner.SummonMonster(hit.point, _monsterID);
                _summonGage.UseSP(_cost);
                Destroy(gameObject);
            }
        }
        // ドラッグ前の位置に戻す
        transform.position = _prevPos;
    }
}
