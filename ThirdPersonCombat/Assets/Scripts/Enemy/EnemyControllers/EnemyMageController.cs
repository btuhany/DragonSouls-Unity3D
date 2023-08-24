using Combat;
using States;
using UnityEngine;

public class EnemyMageController : MonoBehaviour
{
    [Header("RoarAttack")]
    [SerializeField] private ParticleSystem _roarMageFX;
    [SerializeField] private GameObject _roarPlayerFX;

    //[Header("SpellCast")]
    //[SerializeField] private ParticleSystem _spellMageFX;

    [Header("ScytheCast")]
    [SerializeField] private TrailRenderer _sychteTrail;
    public void RoarAttack(int eventNum)
    {
        switch (eventNum)
        {
            case 0:
                _roarMageFX.Stop();
                _roarMageFX.Play();
                break;
            case 1:
                Instantiate(_roarPlayerFX, PlayerStateMachine.Instance.transform.position + Vector3.up * -0.21f, Quaternion.identity);
                break;
            default:
                break;
        }
    }
    //public void SpellCast()
    //{
    //    _spellMageFX.Play();
    //}
    public void SetSycteTrail(int active)
    {
        if(active == 0)
        {
            _sychteTrail.gameObject.SetActive(true);
        }
        else
        {
            _sychteTrail.gameObject.SetActive(false);
        }
    }
}
