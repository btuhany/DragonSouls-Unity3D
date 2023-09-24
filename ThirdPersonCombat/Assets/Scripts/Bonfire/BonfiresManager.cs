using System.Collections.Generic;
using UnityEngine;

public class BonfiresManager : MonoBehaviour
{
    [HideInInspector] public Bonfire lastInteractedBonfire;
    public List<Bonfire> kindledBonfiresList = new List<Bonfire>();
    public static BonfiresManager Instance;
    public event System.Action OnTakeRestEvent;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    public void RegisterKindledBonfire(Bonfire newBonfire)
    {
        kindledBonfiresList.Add(newBonfire);
    }
    public void RestTaken()
    {
        OnTakeRestEvent?.Invoke();
    }
}
