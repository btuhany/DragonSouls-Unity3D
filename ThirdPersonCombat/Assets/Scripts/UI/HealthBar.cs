using UnityEngine;
using UnityEngine.UI;
using Combat;
using System.Collections;
using TMPro;
namespace UIControllers
{
    public class HealthBar : MonoBehaviour
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
            _slider.gameObject.SetActive(false);
            _textDamage.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _health.OnHealthUpdated -= HandleOnHealthUpdated;
        }

        private void HandleOnHealthUpdated(int newHealth, int damage)
        {
            _damageSum += damage;
            _slider.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(LookCamera(_timeDuration));
            _slider.value = (float)newHealth / _health.maxHealth;
            _textHealth.SetText(newHealth + "/" + _health.maxHealth);
            _textDamage.SetText(_damageSum.ToString());
            StartCoroutine(DamageText(_timeDuration / 2));
        }

        IEnumerator LookCamera(float duration)
        {
            float timeCounter = 0f;
            while (timeCounter < duration)
            {
                transform.rotation = Camera.main.transform.rotation;
                timeCounter += Time.deltaTime;
                yield return null;
            }
            _slider.gameObject.SetActive(false);
            yield return null;
        }

        IEnumerator DamageText(float duration)
        {
            _textDamage.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _textDamage.gameObject.SetActive(false);
            yield return null;
        }
    }
}

