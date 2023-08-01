using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UIControllers;
using UnityEditor;

public class TargetableCheck : MonoBehaviour
{
    [Header("TargetCameraConfig")]
    [SerializeField] private float targetMemberCamWeight = 1.0f;
    [SerializeField] private float targetMemberCamRadius = 2.0f;
    [Header("Components")]
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    [SerializeField] private TargetCrosshairController _targetCrosshair;
    private Camera _mainCam;
    private Targetable _currentTargetable;
    private Targetable _previousTargetable;
    public List<Targetable> Targets = new List<Targetable>();
    public bool IsThereTarget => Targets.Count > 0;
    public Transform CurrentTargetTransform => _currentTargetable?.TargetPoint;
    private void Awake()
    {
        _mainCam = Camera.main;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Targetable target))
        {
            Targets.Add(target);
            target.OnDestroyedDisabled += HandleOnTargetDestroyedDisabled;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Targetable target))
        {
            RemoveTarget(target);
        }
    }
    public bool TrySelectTarget()
    {
        if (Targets.Count == 0) return false;
        _currentTargetable = ClosestTargetScreenOrigin();
        if (_currentTargetable == null) return false;
        _targetCrosshair.SetTargetState(_currentTargetable.TargetPoint);
        _cinemachineTargetGroup.AddMember(_currentTargetable.TargetPoint, targetMemberCamWeight, targetMemberCamRadius);
        return true;
    }
    public bool TryTransferTarget()
    {
        if (Targets.Count == 0) return false;
        _currentTargetable = _previousTargetable;
        if (_currentTargetable == null) return false;
        _targetCrosshair.SetTargetState(_currentTargetable.TargetPoint);
        _cinemachineTargetGroup.AddMember(_previousTargetable.TargetPoint, targetMemberCamWeight, targetMemberCamRadius);
        return true;
    }
    public void ClearTarget()
    {
        if (!_currentTargetable) return;
        _cinemachineTargetGroup.RemoveMember(_currentTargetable.TargetPoint);
        _previousTargetable = _currentTargetable;
        _currentTargetable = null;
        _targetCrosshair.ClearTarget();
    }
    public bool IsTargetInRange()
    {
        if (!_currentTargetable) return false;
        foreach (Targetable targetable in Targets)
        {
            if (targetable == _currentTargetable)
            {
                return true;
            }
        }
        return false;
    }
    private Targetable ClosestTargetScreenOrigin()
    {
        //Assigning the value of max point to center distance which is 0.7f
        float minCenterDistance = 1.5f;
        Targetable closestTarget = null;

        foreach (Targetable target in Targets)
        {
            Vector2 viewPos = _mainCam.WorldToViewportPoint(target.transform.position);
            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) continue;

            //Screen origin is Vector2(0.5f,0.5f)
            float distance = Vector2.Distance(viewPos, Vector2.one / 2);

            if(distance <= minCenterDistance)
            {
                closestTarget = target;
                minCenterDistance = distance;
            }
        }
        return closestTarget;
    }
    public Targetable GetClosestTarget()
    {
        if (!IsThereTarget) return null;
        //Assigning the half value of max distance which is radius / 2.
        float minDistance = _collider.radius / 2f;
        int closestTargetIndex = -1;
        for (int i = 0; i < Targets.Count; i++)
        {
            float distance = Vector3.Distance(_playerTransform.position, Targets[i].transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                closestTargetIndex = i;
            }
        }
        if (closestTargetIndex >= 0)
            return Targets[closestTargetIndex];
        else
            return null;
    }

    private void HandleOnTargetDestroyedDisabled(Targetable target)
    {
        RemoveTarget(target);
    }
    
    private void RemoveTarget(Targetable target)
    {
        if (target == _currentTargetable)
            ClearTarget();
        Targets.Remove(target);
        target.OnDestroyedDisabled -= HandleOnTargetDestroyedDisabled;
    }
    
    public void ChangeTarget(Vector2 selectDir)
    {
        Targetable closestTarget = null;
        float closestDistance = 500f;
        foreach (Targetable target in Targets)
        {
            if (target == _currentTargetable) continue;
            Vector2 viewPos = _mainCam.WorldToViewportPoint(target.transform.position);
            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) continue;

            Vector2 targetPos = _mainCam.WorldToViewportPoint(_currentTargetable.TargetPoint.position);
            //Screen origin is Vector2(0.5f,0.5f)
            Vector2 distanceVector = viewPos - targetPos;
            float distance = distanceVector.magnitude;
            if (Vector2.Dot(distanceVector.normalized, selectDir.normalized) > 0f)
            {
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }
        if (closestTarget != null)
        {
            ClearTarget();
            _currentTargetable = closestTarget;
            _targetCrosshair.SetTargetState(_currentTargetable.TargetPoint);
            _cinemachineTargetGroup.AddMember(_currentTargetable.TargetPoint, targetMemberCamWeight, targetMemberCamRadius);
        }
    }

}
