using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterScramble
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _playerTran;
        [SerializeField] private Vector3 _offset;

        void LateUpdate()
        {
            transform.position = _playerTran.position + _offset;
        }
    }
}