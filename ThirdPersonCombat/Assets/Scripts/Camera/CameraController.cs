using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineBrain _cinemachineBrain;
    public CinemachineStateDrivenCamera _cinemachineStateDrivenCam;
    public CinemachineFreeLook _cinemachineFreeLookCam;
    public CinemachineVirtualCamera _cinemachineAimCam;
    public CinemachineRecomposer _cinemachineAimCamRecomposer;

    public bool IsAimCameraActive => _cinemachineStateDrivenCam.LiveChild.Priority == _cinemachineAimCam.Priority;
    public bool IsTransition => _cinemachineStateDrivenCam.IsBlending;

    public void SetAimCamTarget(Transform targetTransform)
    {
        _cinemachineAimCam.LookAt = targetTransform;
        _cinemachineAimCamRecomposer.m_Pan = 0;
    }
    public void ResetAimCamTarget()
    {
        _cinemachineAimCam.LookAt = null;
        _cinemachineAimCamRecomposer.m_Pan = -6;
    }
}
