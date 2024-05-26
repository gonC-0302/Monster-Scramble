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
    private BattleStateManager _stateManager;
    private AttackManager _playerAttack;
    private int _playerID;
    private float _attackTargetOffset = 2f;
    private static readonly string FieldLayerName = "Field";

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        _stateManager = GetComponent<BattleStateManager>();
        _playerAttack = GetComponent<AttackManager>();
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
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        if (Object.HasStateAuthority)
        {
            _distance = Vector3.Distance(_targetPos, transform.position);

            switch (_stateManager.CurrentState)
            {
                case BattleState.Idle:
                    TrySetTargetTran();
                    break;
                case BattleState.Move:
                    TrySetTargetTran();
                    if (_distance < 1)
                    {
                        _stateManager.SwitchState(BattleState.Idle);
                        return;
                    }
                    _direction = (_targetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case BattleState.MoveToTarget:
                    TrySetTargetTran();
                    if (_distance < _attackTargetOffset)
                    {
                        _stateManager.SwitchState(BattleState.PreparateAttack);
                        return;
                    }
                    _direction = (_targetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case BattleState.PreparateAttack:
                    TrySetTargetTran();
                    transform.LookAt(_targetPos);
                    break;
                case BattleState.Attack:
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
            Ray ray2D = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)Input.mousePosition, (Vector2)ray2D.direction);
            if (hit2d) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out IDamagable target))
                {
                    if (_playerID == target.TeamID) return;
                    _stateManager.SwitchState(BattleState.MoveToTarget);
                    _targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
                {
                    _stateManager.SwitchState(BattleState.Move);
                    _targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
            }
        }
    }
}
