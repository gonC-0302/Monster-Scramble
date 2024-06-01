using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private PlayerCameraMovement _camera;
    private NetworkCharacterController characterController;
    private Transform _attackTargetTran;
    private Vector3 _moveTargetPos;
    private Vector3 _direction;
    private float _distance;
    private CharacterStateMachine _stateManager;
    private int _playerID;
    private static readonly string FieldLayerName = "Field";

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        _stateManager = GetComponent<CharacterStateMachine>();
    }

    public override void Spawned()
    {
        // 自分自身のアバターにカメラを追従させる
        if (Object.HasStateAuthority)
        {
            _camera.SetCameraTarget();
            _attackTargetTran = transform;
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
            switch (_stateManager.CurrentState)
            {
                case CharacterState.Idle:
                case CharacterState.PreparateAttack:
                    TrySetTargetTran();
                    break;
                case CharacterState.Move:
                    TrySetTargetTran();
                    _distance = Vector3.Distance(_moveTargetPos, transform.position);
                    if (_distance < 1)
                    {
                        _stateManager.SwitchState(CharacterState.Idle);
                        return;
                    }
                    _direction = (_moveTargetPos - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case CharacterState.MoveToTarget:
                    TrySetTargetTran();
                    _direction = (_attackTargetTran.position - transform.position).normalized;
                    characterController.Move(_direction);
                    break;
                case CharacterState.Attack:
                    _attackTargetTran = transform;
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
                    _stateManager.SwitchState(CharacterState.MoveToTarget);
                    _attackTargetTran = hit.collider.gameObject.transform;
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
                {
                    _stateManager.SwitchState(CharacterState.Move);
                    _moveTargetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }
            }
        }
    }
}
