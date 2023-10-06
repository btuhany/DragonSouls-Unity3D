using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfiresManager : MonoBehaviour
{
    [SerializeField] private BonfirePanel _bonfirePanel;
    [HideInInspector] public Bonfire LastInteractedBonfire 
    { 
        get => _lastInteractedBonfire;
        set 
        {
            if(_lastInteractedBonfire != null)
                _cinemachineBonfireTargetGroup.RemoveMember(_lastInteractedBonfire.transform);
            _lastInteractedBonfire = value;
            _cinemachineBonfireTargetGroup.AddMember(_lastInteractedBonfire.transform, 2f, 0f);
        }
    }
    public List<Bonfire> kindledBonfiresList = new List<Bonfire>();
    public static BonfiresManager Instance;
    public event System.Action OnTakeRestEvent;
    [Header("Camera")]
    [SerializeField] private CinemachineTargetGroup _cinemachineBonfireTargetGroup;
    private Bonfire _lastInteractedBonfire;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
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
        StartCoroutine(BonfirePanelDelay());
        OnTakeRestEvent?.Invoke();
    }
    public void ExitRest()
    {
        _bonfirePanel.gameObject.SetActive(false);
    }
    WaitForSeconds _bonfirePanelDelay = new WaitForSeconds(2.7f);
    private IEnumerator BonfirePanelDelay()
    {
        yield return _bonfirePanelDelay;
        _bonfirePanel.gameObject.SetActive(true);
        yield return null;
    }
}
