using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public void SetCameraTarget(Transform targetTran)
    {
        var freeLookCamera = FindObjectOfType<CinemachineVirtualCamera>();
        freeLookCamera.LookAt = targetTran;
        freeLookCamera.Follow = targetTran;
    }
}
