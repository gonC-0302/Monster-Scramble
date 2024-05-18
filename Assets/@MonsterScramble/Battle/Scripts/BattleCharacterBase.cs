using System.Collections;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterBase : NetworkBehaviour
{
    private CharacterController _characterController;
    private float _colRadius;
    public int PlayerID => _playerID;
    protected int _playerID;
    private HitPoint _hp;

    private void Awake()
    {
        _hp = GetComponent<HitPoint>();
        _characterController = GetComponent<CharacterController>();
    }
    public float GetAttackOffset()
    {
        return _characterController.radius - 0.5f;
    }
    public void SetPlayerID(int playerID)
    {
        _playerID = playerID;
    }
    public void GetHit(float damage)
    {
        _hp.DealDamageRpc(damage);
    }
}
