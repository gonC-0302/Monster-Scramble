using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterScramble
{
    [RequireComponent(typeof(PlayerStateManager))]
    [RequireComponent(typeof(PlayerAnimation))]
    public class PlayerAttack : MonoBehaviour
    {
        private PlayerAnimation _playerAnim;
        private PlayerStateManager _stateManager;
        private float _timer;
        private HitPoint _targetHP;
        private Transform _targetTran;
        [SerializeField] private float _attackInterval;
        [SerializeField] private int _attackPower;
        void Start()
        {
            _playerAnim = GetComponent<PlayerAnimation>();
            _stateManager = GetComponent<PlayerStateManager>();
        }

        public void SetTarget(Transform targetTran ,HitPoint targetHP)
        {
            this._targetTran = targetTran;
            this._targetHP = targetHP;
            _stateManager.SwitchState(State.Attack);
        }

        void Update()
        {
            switch (_stateManager.CurrentState)
            {
                case State.PreparateAttack:
                    transform.LookAt(_targetTran);
                    PreparateAttack();
                    break;
                case State.Attack:
                    transform.LookAt(_targetTran);
                    Attack();
                    break;
            }
        }

        /// <summary>
        /// 攻撃準備
        /// </summary>
        private void PreparateAttack()
        {
            _timer += Time.deltaTime;
            if (_timer > _attackInterval)
            {
                _timer = 0;
                _stateManager.SwitchState(State.Attack);
                if (!IsExitTarget()) _stateManager.SwitchState(State.Move);
            }
        }

        private bool IsExitTarget()
        {
            return _targetHP != null ? true : false;
        }

        /// <summary>
        /// 攻撃
        /// </summary>
        private void Attack()
        {
            _targetHP.GetHit(_attackPower);
            _playerAnim.PlayAttackAnimation();
            _stateManager.SwitchState(State.PreparateAttack);
        }
    }
}