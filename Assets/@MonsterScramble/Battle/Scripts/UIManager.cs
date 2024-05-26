using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    private TextMeshProUGUI _timerText;

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

    public void UpdateBattleTimer(float time)
    {
        var span = new TimeSpan(0, 0, (int)time);
        _timerText.text = span.ToString(@"mm\:ss");
    }
}
