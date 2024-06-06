using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkCharacterController))]
[RequireComponent(typeof(CharacterStateMachine))]
[RequireComponent(typeof(CursolMovement))]
public class PlayerMovement : NetworkBehaviour
{
    [Networked]
    private int TeamID { get; set; }
    private CursolMovement _cursol;
    private NetworkCharacterController _characterController;
    private CharacterStateMachine _stateMachine;
    private Transform _attackTargetTran;
    private Vector3 _moveTargetPos;
    private float _distance;
    private Vector3 _direction;
    private static readonly string FieldLayerName = "Field";

    /// <summary>
    /// 初期設定
    /// </summary>
    public override void Spawned()
    {
        if (!Object.HasStateAuthority) return;
        _characterController = GetComponent<NetworkCharacterController>();
        _stateMachine = GetComponent<CharacterStateMachine>();
        _cursol = GetComponent<CursolMovement>();
        TeamID = Runner.LocalPlayer.PlayerId;
        _attackTargetTran = transform;
        _cursol.Initialize();
    }
    private void Update()
    {
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        if (!Object.HasStateAuthority) return;

        switch (_stateMachine.CurrentState)
        {
            case CharacterState.Idle:
                _cursol.Move(transform.position, false);
                TrySetTargetTran();
                break;
            case CharacterState.PreparateAttack:
                TrySetTargetTran();
                break;
            case CharacterState.Move:
                TrySetTargetTran();
                break;
            case CharacterState.MoveToTarget:
                _cursol.Move(_attackTargetTran.position, true);
                TrySetTargetTran();
                break;
            case CharacterState.Attack:
                break;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GameManager.instance.CurrentGameState != GameState.Battle) return;
        if (!Object.HasStateAuthority) return;
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        switch (_stateMachine.CurrentState)
        {
            case CharacterState.Move:
                _distance = Vector3.Distance(_moveTargetPos, transform.position);
                if (_distance < 0.1f)
                {
                    _stateMachine.SwitchState(CharacterState.Idle);
                    return;
                }
                _direction = (_moveTargetPos - transform.position).normalized;
                _characterController.Move(_direction);
                break;
            case CharacterState.MoveToTarget:
                _distance = Vector3.Distance(transform.position, _attackTargetTran.position);
                if (_distance < 2)
                {
                    _stateMachine.SwitchState(CharacterState.PreparateAttack);
                    return;
                }
                _direction = (_attackTargetTran.position - transform.position).normalized;
                _characterController.Move(_direction);
                break;
        }
    }
    /// <summary>
    /// 敵に衝突したら攻撃準備
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_stateMachine.CurrentState == CharacterState.Death) return;
        if (hit.gameObject.TryGetComponent(out IDamagable target))
        {
            if (target.TeamID == TeamID) return;
            transform.LookAt(hit.gameObject.transform);
            _stateMachine.SwitchState(CharacterState.PreparateAttack);
            _cursol.Move(hit.gameObject.transform.position, true);
        }
    }
    /// <summary>
    /// タップした場所に目的地を登録
    /// </summary>
    public void TrySetTargetTran()
    {
        if (Input.GetMouseButtonDown(0))
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
                    if (TeamID == target.TeamID) return;
                    _stateMachine.SwitchState(CharacterState.MoveToTarget);
                    _attackTargetTran = hit.collider.gameObject.transform;
                    _cursol.Move(hit.collider.gameObject.transform.position,true);
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
                {
                    _stateMachine.SwitchState(CharacterState.Move);
                    _moveTargetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    _cursol.Move(hit.point,false);
                }
            }
        }
    }
}
