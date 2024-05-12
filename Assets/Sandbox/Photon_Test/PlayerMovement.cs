using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    static readonly string FieldLayerName = "Field";
    static readonly string EnemyLayerName = "Enemy";
    private Vector3 _targetPos;
    public Vector3 TargetPos => new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
    private Vector3 _direction;
    private float _distance;
    [SerializeField]
    private PlayerCameraMovement _camera;
    private PlayerStateManager _stateManager;
    private PlayerAttack _playerAttack;

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
                    _playerAttack.SetAttackTarget(hp);
                    _stateManager.SwitchState(State.MoveToEnemy);
                    _targetPos = hit.point;
                }
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
                {
                    _stateManager.SwitchState(State.Move);
                    _targetPos = hit.point;
                }
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            _distance = Vector3.Distance(TargetPos, transform.position);

            switch (_stateManager.CurrentState)
            {
                case State.Idle:
                case State.Move:
                    TrySetTargetTran();
                    if (_distance < 1)
                    {
                        _stateManager.SwitchState(State.Idle);
                        return;
                    }
                    _direction = (TargetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case State.MoveToEnemy:
                    TrySetTargetTran();
                    if (_distance < 1)
                    {
                        _stateManager.SwitchState(State.PreparateAttack);
                        return;
                    }
                    _direction = (TargetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case State.PreparateAttack:
                    TrySetTargetTran();
                    transform.LookAt(TargetPos);
                    break;
                case State.Attack:
                    _targetPos = transform.position;
                    break;
            }
        }
    }
}
