using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummonGage : MonoBehaviour
{
    private int _chargedSP;
    private float _timer;
    private const float _interval = 10;

    void Update()
    {
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        _timer += Time.deltaTime;
        if (_timer > _interval)
        {
            _timer = 0;
            ChargeSP();
        }
        UIManager.instance.UpdateSPGage(_timer/_interval);
    }
    private void ChargeSP()
    {
        _chargedSP++;
        UIManager.instance.UpdateSPText(_chargedSP);
    }
    public void UseSP(int value)
    {
        _chargedSP -= value;
        UIManager.instance.UpdateSPText(_chargedSP);
    }
    public int GetChargedSP()
    {
        return _chargedSP;
    }
}
