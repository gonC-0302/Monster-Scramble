using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;

public class BattleTimeManager : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(TimerChanged))]
    private float _battleTimer { get; set; }
    private NetworkRunner _runner;
    private const int _battleTimeMinutes = 3;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            _battleTimer = _battleTimeMinutes * 60;
        }
    }

    public void Init(NetworkRunner runner)
    {
        _runner = runner;
    }
   
    void TimerChanged()
    {
        UIManager.instance.UpdateBattleTimer(_battleTimer);
        if (_battleTimer <= 0)
        {
            Debug.Log("ゲーム終了");
            GameManager.instance.SwitchGameState(GameState.Finish);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        DealBattleTimerRpc();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public virtual void DealBattleTimerRpc()
    {
        _battleTimer -= _runner.DeltaTime;
    }
}
