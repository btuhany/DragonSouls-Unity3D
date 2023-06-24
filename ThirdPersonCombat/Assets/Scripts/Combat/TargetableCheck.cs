using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using System;

public class TargetableCheck : MonoBehaviour
{
    [Header("TargetCameraConfig")]
    [SerializeField] private float targetMemberCamWeight = 1.0f;
    [SerializeField] private float targetMemberCamRadius = 2.0f;
    [Header("Components")]
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
    private Camera _mainCam;
    private Targetable _currentTargetable;
    public List<Targetable> Targets = new List<Targetable>();
    public Transform CurrentTargetTransform => _currentTargetable.transform;
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
        _cinemachineTargetGroup.AddMember(_currentTargetable.transform, targetMemberCamWeight, targetMemberCamRadius);
        return true;
    }
    public void ClearTarget()
    {
        if (!_currentTargetable) return;
        _cinemachineTargetGroup.RemoveMember(_currentTargetable.transform);
        _currentTargetable = null;
    }
    public bool IsTargetInRange()
    {
        if (!_currentTargetable) return false;
        foreach (Targetable targetable in Targets)
        {
            if (targetable == _currentTargetable)
                return true;
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
    private Targetable ClosestTarget()
    {
        //Assigning the value of max distance which is radius * 2.
        float minDistance = _collider.radius * 2;
        int closestTargetIndex = 0;
        for (int i = 0; i < Targets.Count; i++)
        {
            float distance = Vector3.Distance(_playerTransform.position, Targets[i].transform.position);
            if (distance <= minDistance)
            {
                minDistance = distance;
                closestTargetIndex = i;
            }
        } 
        return Targets[closestTargetIndex];
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

}
