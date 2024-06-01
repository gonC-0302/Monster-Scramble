using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    private TextMeshProUGUI _timerText;
    [SerializeField]
    private Transform _startLogoTran;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayStartLogoAnimation()
    {
        _startLogoTran.transform.localScale = Vector3.zero;
        _startLogoTran.transform.DOScale(1.25f, 1f).OnComplete(() => _startLogoTran.gameObject.SetActive(false));
    }

    public void UpdateBattleTimer(float time)
    {
        var span = new TimeSpan(0, 0, (int)time);
        _timerText.text = span.ToString(@"mm\:ss");
    }
}
