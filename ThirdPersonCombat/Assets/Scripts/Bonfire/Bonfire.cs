using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fireFX;
    [SerializeField] private ParticleSystem _fireCircleFX;
    [SerializeField] private GameObject _uiUnkindledInfo;
    [SerializeField] private GameObject _uiKindledInfo;
    public Transform respawnPoint;
    [HideInInspector] public bool isLighted;


    public void KindleBonfire()
    {
        if (isLighted) return;
        _fireFX.Play();
        isLighted = true;
        _uiUnkindledInfo.SetActive(false);
        _uiKindledInfo.SetActive(true);
        BonfiresManager.Instance.RegisterKindledBonfire(this);
        BonfiresManager.Instance.LastInteractedBonfire = this;
    }
    public void TakeRest()
    {
        BonfiresManager.Instance.LastInteractedBonfire = this;
        PlayerStateMachine.Instance.health.ResetHealth();
        PlayerStateMachine.Instance.stamina.ResetStamina();
        _uiKindledInfo.SetActive(false);
        BonfiresManager.Instance.RestTaken();
        _fireCircleFX.Play();
    }
    public void AtBonfire()
    {
        if (isLighted)
        {
            _uiKindledInfo.SetActive(true);
        }
        else
        {
            _uiUnkindledInfo.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            States.PlayerStateMachine.Instance.currentBonfire = this;
            if (isLighted)
            {
                _uiKindledInfo.SetActive(true);
            }
            else
            {
                _uiUnkindledInfo.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine.Instance.currentBonfire = null;
            if (isLighted)
            {
                _uiKindledInfo.SetActive(false);
            }
            else
            {
                _uiUnkindledInfo.SetActive(false);
            }
                
        }
    }

}
