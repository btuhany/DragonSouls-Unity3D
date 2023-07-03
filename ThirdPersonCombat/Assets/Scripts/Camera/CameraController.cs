using Cinemachine;
using Inputs;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineBrain _cinemachineBrain;
    public CinemachineStateDrivenCamera _cinemachineStateDrivenCam;
    public CinemachineFreeLook _cinemachineFreeLookCam;
    public CinemachineVirtualCamera _cinemachineAimCam;
    public CinemachineRecomposer _cinemachineAimCamRecomposer;

    public Transform _aimCamFocus;

    public bool IsAimCameraActive => _cinemachineStateDrivenCam.LiveChild.Priority == _cinemachineAimCam.Priority;
    public bool IsTransition => _cinemachineStateDrivenCam.IsBlending;

    public void SetAimCamTarget(Transform targetTransform, Vector3 targetDir)
    {
        _aimCamFocus.rotation = Quaternion.LookRotation(targetDir);
        _cinemachineAimCam.LookAt = targetTransform;
        _cinemachineAimCamRecomposer.m_Pan = 0;
    }
    public void ResetAimCamTarget()
    {
        _cinemachineAimCam.LookAt = null;
        _cinemachineAimCamRecomposer.m_Pan = -6;
    }
    public void AimCamRotation(float value)
    {
        _aimCamFocus.rotation *= Quaternion.AngleAxis(value, Vector3.left);
    }
    public void AimCamSetVerticalRotation(float value)
    {
        _aimCamFocus.localRotation = Quaternion.AngleAxis(value, Vector3.right);
    }
}
