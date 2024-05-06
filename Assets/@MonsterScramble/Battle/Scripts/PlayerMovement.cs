using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerAnimation))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private VariableJoystick _variableJoystick;
    private PlayerAnimation _playerAnim;
    private Rigidbody _rb;
    private Transform _transform;
    private Vector3 _prevPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerAnim = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        _transform = transform;
        _prevPosition = _transform.position;
    }

    private void Update()
    {
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = Vector3.forward * _variableJoystick.Vertical + Vector3.right * _variableJoystick.Horizontal;
        _rb.AddForce(direction * _speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        _playerAnim.PlayMoveAnimation(direction.magnitude);
    }

    /// <summary>
    /// 移動方向に滑らかに回転させる
    /// </summary>
    private void Rotate()
    {
        var position = _transform.position;
        var delta = position - _prevPosition;
        _prevPosition = position;
        if (delta == Vector3.zero)
            return;
        var rotation = Quaternion.LookRotation(delta, Vector3.up);
        _transform.rotation = rotation;
    }
}
