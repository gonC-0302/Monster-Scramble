using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MonsterScramble
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerStateManager))]
    [RequireComponent(typeof(PlayerAnimation))]
    [RequireComponent(typeof(PlayerAttack))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private VariableJoystick _variableJoystick;
        private PlayerAnimation _playerAnim;
        private PlayerAttack _playerAttack;
        private PlayerStateManager _stateManager;
        private Rigidbody _rb;
        private Vector3 _prevPosition;
        private Vector3 _targetPos;
        static readonly string FieldLayerName = "Field";
        static readonly string EnemyLayerName = "Enemy";


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _playerAnim = GetComponent<PlayerAnimation>();
            _playerAttack = GetComponent<PlayerAttack>();
            _stateManager = GetComponent<PlayerStateManager>();
        }

        private void Start()
        {
            _targetPos = transform.position;
            _prevPosition = transform.position;
        }

        private void Update()
        {
            //#if UNITY_EDITOR
            //            if (EventSystem.current.IsPointerOverGameObject())
            //            {
            //                return;
            //            }
            //#else 
            //    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            //        return;
            //    }
            //#endif
            if (_stateManager.CurrentState == State.Summon) return;
            TrySetTargetTran();
            Rotate();
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
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer(FieldLayerName))
                    {
                        _stateManager.SwitchState(State.Move);
                        _targetPos = hit.point;
                    }
                    if(hit.collider.gameObject.layer == LayerMask.NameToLayer(EnemyLayerName))
                    {
                        _stateManager.SwitchState(State.Move);
                        _targetPos = hit.point;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (_stateManager.CurrentState == State.Move || _stateManager.CurrentState == State.Summon)
            {
                var distance = Vector3.Distance(_targetPos, transform.position);
                if (distance < 1)
                {
                    _playerAnim.PlayMoveAnimation(0);
                    return;
                }
                Move();
            }
        }

        private void Move()
        {
            var direction = (_targetPos - transform.position).normalized;
            _rb.AddForce(direction * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
            _playerAnim.PlayMoveAnimation(direction.magnitude);
        }

        /// <summary>
        /// 移動方向に滑らかに回転させる
        /// </summary>
        private void Rotate()
        {
            var position = transform.position;
            var delta = position - _prevPosition;
            _prevPosition = position;
            if (delta == Vector3.zero)
                return;
            var rotation = Quaternion.LookRotation(delta, Vector3.up);
            transform.rotation = rotation;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_stateManager.CurrentState != State.Move) return;
            if (collision.gameObject.TryGetComponent<HitPoint>(out HitPoint hp))
            {
                _playerAnim.PlayMoveAnimation(0);
                _targetPos = transform.position;
                _playerAttack.SetTarget(collision.gameObject.transform, hp);
            }
        }
    }
}