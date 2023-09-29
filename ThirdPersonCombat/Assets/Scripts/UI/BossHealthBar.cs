using Combat;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] float _timeDuration = 3f;
    [SerializeField] Slider _slider;
    [SerializeField] Health _health;
    [SerializeField] TextMeshProUGUI _textHealth;
    [SerializeField] TextMeshProUGUI _textDamage;
    private int _damageSum = 0;

    private void OnEnable()
    {
        _health.OnHealthUpdated += HandleOnHealthUpdated;
        _textDamage.gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        _health.OnHealthUpdated -= HandleOnHealthUpdated;
    }

    private void HandleOnHealthUpdated(int newHealth, int damage)
    {
        _damageSum += damage;
        _slider.value = (float)newHealth / _health.maxHealth;
        _textDamage.SetText(_damageSum.ToString());
        StopAllCoroutines();
        StartCoroutine(DamageText(_timeDuration / 2));
        Debug.Log("boss");
    }

    IEnumerator DamageText(float duration)
    {
        _textDamage.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        _textDamage.gameObject.SetActive(false);
        _damageSum = 0;
        yield return null;
    }
}
