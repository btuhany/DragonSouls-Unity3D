using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class FreeLookCameraController : MonoBehaviour
{
    [SerializeField] public CinemachineFreeLook FreeLookCamera;
    public void OnFreeLookCam()
    {
        FreeLookCamera.m_RecenterToTargetHeading.m_enabled = true;
        StopAllCoroutines();
        StartCoroutine(SetRecenterFalse());
    }
    IEnumerator SetRecenterFalse()
    {
       float waitTime = FreeLookCamera.m_RecenterToTargetHeading.m_WaitTime;
       float recenteringTime = FreeLookCamera.m_RecenterToTargetHeading.m_RecenteringTime;
       yield return new WaitForSeconds(waitTime + recenteringTime);
       FreeLookCamera.m_RecenterToTargetHeading.m_enabled = false;
    }
}
