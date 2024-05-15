using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private void Awake()
    {
        
    }

    public void SetCameraTarget()
    {
        var freeLookCamera = FindObjectOfType<CinemachineVirtualCamera>();
        freeLookCamera.LookAt = transform;
        freeLookCamera.Follow = transform;
    }

    
}
