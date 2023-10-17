using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocuser : MonoBehaviour
{
    private CinemachineVirtualCamera m_VirtualCam;

    private void Awake()
    {
        m_VirtualCam = GetComponent<CinemachineVirtualCamera>();      
    }

    private void Update()
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        m_VirtualCam.LookAt = target;
        m_VirtualCam.Follow = target;
    }
}
