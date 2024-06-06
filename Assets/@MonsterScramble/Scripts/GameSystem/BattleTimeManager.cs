using UnityEngine;
using Fusion;

public class BattleTimeManager : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(TimerChanged))]
    private float _battleTimer { get; set; }
    private const int _battleTimeMinutes = 5;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority) return;
        _battleTimer = _battleTimeMinutes * 60;
    }
   
    void TimerChanged()
    {
        UIManager.instance.UpdateBattleTimer(_battleTimer);
        if (_battleTimer <= 0)
        {
            Debug.Log("ゲーム終了");
            GameManager.instance.Draw();
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
        _battleTimer -= Runner.DeltaTime;
    }
}
