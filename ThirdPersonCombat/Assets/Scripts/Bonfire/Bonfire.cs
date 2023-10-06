using States;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private GameObject _light;
    [SerializeField] private ParticleSystem _fireFX;
    [SerializeField] private ParticleSystem _fireCircleFX;
    [SerializeField] private GameObject _uiUnkindledInfo;
    [SerializeField] private GameObject _uiKindledInfo;
    public Transform respawnPoint;
    [HideInInspector] public bool isLighted;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void KindleBonfire()
    {
        if (isLighted) return;
        SoundManager.Instance.PlayAuidoClip(1, 0);
        _audioSource.Play();
        _fireFX.Play();
        isLighted = true;
        _uiUnkindledInfo.SetActive(false);
        _uiKindledInfo.SetActive(true);
        _light.SetActive(true);
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
