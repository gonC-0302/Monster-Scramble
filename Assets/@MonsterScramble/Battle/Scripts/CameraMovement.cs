using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerTran;
    [SerializeField] private Vector3 _offset;

    void Update()
    {
        transform.position = _playerTran.position + _offset;
    }
}
