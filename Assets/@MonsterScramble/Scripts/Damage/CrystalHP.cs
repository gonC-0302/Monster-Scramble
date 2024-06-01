using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class CrystalHP : NetworkBehaviour, IDamagable
{
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    public bool IsSealedMonster { get; set; }
    public int MaxHP { get; set; }
    [SerializeField]
    private GameObject _hpCanvas;
    [SerializeField]
    private Image _hpGage;

    public void Init(int teamID)
    {
        TeamID = teamID;
    }
    public void UpdateHPGage()
    {
        _hpGage.fillAmount = NetworkedHP / MaxHP;
    }
    public override void FixedUpdateNetwork()
    {
        _hpCanvas.transform.LookAt(Camera.main.transform);
    }


    void HealthChanged()
    {
        UpdateHPGage();
        if (NetworkedHP < 1)
        {
            GameManager.instance.SwitchGameState(GameState.Finish);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damagedHP)
    {
        NetworkedHP = damagedHP;
    }
}
