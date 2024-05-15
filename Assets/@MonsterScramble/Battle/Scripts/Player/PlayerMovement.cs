using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private PlayerCameraMovement _camera;
    private NetworkCharacterController characterController;
    private Vector3 _targetPos;
    private Vector3 _direction;
    private float _distance;
    private PlayerStateManager _stateManager;
    private PlayerAttack _playerAttack;
    private int _playerID;
    private static readonly string FieldLayerName = "Field";

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        _stateManager = GetComponent<PlayerStateManager>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    public override void Spawned()
    {
        // 自分自身のアバターにカメラを追従させる
        if (Object.HasStateAuthority)
        {
            _camera.SetCameraTarget();
            _targetPos = transform.position;
        }
    }

    public void SetPlayerID(int playerID)
    {
        _playerID = playerID;
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            _distance = Vector3.Distance(_targetPos, transform.position);

            switch (_stateManager.CurrentState)
            {
                case PlayerState.Idle:
                    TrySetTargetTran();
                    break;
                case PlayerState.Move:
                    TrySetTargetTran();
                    if (_distance < 1)
                    {
                        _stateManager.SwitchState(PlayerState.Idle);
                        return;
                    }
                    _direction = (_targetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case PlayerState.MoveToEnemy:
                    TrySetTargetTran();
                    if (_distance < 1)
                    {
                        _stateManager.SwitchState(PlayerState.PreparateAttack);
                        return;
                    }
                    _direction = (_targetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case PlayerState.PreparateAttack:
                    TrySetTargetTran();
                    transform.LookAt(_targetPos);
                    break;
                case PlayerState.Attack:
                    _targetPos = transform.position;
                    break;
            }
        }
    }
    /// <summary>
    /// 敵をタップすると敵の場所まで進む
    /// </summary>
    public void TrySetTargetTran()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out HitPoint hp))
                {
                    if (_playerID == hp.PlayerID) return;

                    _playerAttack.SetAttackTarget(hp);
                    _stateManager.SwitchState(PlayerState.MoveToEnemy);
                    _targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
                {
                    _stateManager.SwitchState(PlayerState.Move);
                    _targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
            }
        }
    }
}
