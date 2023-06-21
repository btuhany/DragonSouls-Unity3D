using System.Collections.Generic;
using UnityEngine;

public class TargetableCheck : MonoBehaviour
{
    public List<Targetable> targets = new List<Targetable>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Targetable target))
        {
            targets.Add(target);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Targetable target))
        {
            targets.Remove(target);
        }
    }
    
}
