using UnityEngine;
using UnityEngine.UI;
using Combat;
using System.Collections;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Health _health;
    [SerializeField] TextMeshProUGUI _text;
    private void OnEnable()
    {
        _health.OnHealthUpdated += HandleOnHealthUpdated;
    }
    private void OnDisable()
    {
        _health.OnHealthUpdated -= HandleOnHealthUpdated;
    }
    private void HandleOnHealthUpdated(int newHealth)
    {
        _slider.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(LookCamera(3f));
        _slider.value = (float)newHealth / _health.MaxHealth;
        _text.SetText(newHealth + "/" + _health.MaxHealth);
    }
    IEnumerator LookCamera(float duration)
    {
        float timeCounter = 0f;
        while(timeCounter<duration)
        {
            transform.rotation = Camera.main.transform.rotation;
            timeCounter += Time.deltaTime;
            yield return null;
        }
        _slider.gameObject.SetActive(false);
        yield return null;
    }
}
