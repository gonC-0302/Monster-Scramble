using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class SummonMonsterHP : NetworkBehaviour, IDamagable
{
    public int TeamID { get; set; }
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHP { get; set; }
    public bool IsSealedMonster { get; set; }
    public int MaxHP { get; set; }
    //[SerializeField]
    //private GameObject _hpCanvas;
    //[SerializeField]
    //private Image _hpGage;

    public void Init(int teamID)
    {
        TeamID = teamID;
    }

    //public override void FixedUpdateNetwork()
    //{
    //    _hpCanvas.transform.LookAt(Camera.main.transform);
    //}

    //public void UpdateHPGage()
    //{
    //    _hpGage.fillAmount = NetworkedHP / MaxHP;
    //}

    void HealthChanged()
    {
        //UpdateHPGage();
        Debug.Log($"Health changed to: {NetworkedHP}");
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage)
    {
        if (NetworkedHP == 0) return;

        NetworkedHP = damage;
        if (NetworkedHP < 1)
        {
            Destroy(gameObject);
        }
    }
}
