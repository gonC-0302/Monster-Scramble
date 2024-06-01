using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class PlayerHP : NetworkBehaviour, IDamagable
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
    private Transform _playerTran;

    public void Init(int teamID,Transform playerTran)
    {
        _playerTran = playerTran;
        TeamID = teamID;
        _hpCanvas.transform.SetParent(null);
    }

    public void UpdateHPGage()
    {
        _hpGage.fillAmount = NetworkedHP / MaxHP;
    }
    public override void FixedUpdateNetwork()
    {
        _hpCanvas.transform.position = new Vector3(_playerTran.position.x, _hpCanvas.transform.position.y, _playerTran.position.z);
        //_hpCanvas.transform.LookAt(Camera.main.transform);
    }

    void HealthChanged()
    {
        Debug.Log($"Health changed to: {NetworkedHP}");
        UpdateHPGage();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage)
    {
        NetworkedHP = damage;

        if (NetworkedHP < 1)
        {
            Destroy(gameObject);
        }
    }
}
