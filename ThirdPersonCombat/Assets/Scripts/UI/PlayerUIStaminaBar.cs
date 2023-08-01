using UnityEngine;
using PlayerController;
using UnityEngine.UI;

namespace PlayerUI
{
    public class PlayerUIStaminaBar : MonoBehaviour
    {
        [SerializeField] private Stamina _playerStamina;
        private Slider _slider;

        private void Awake()
        {
            _playerStamina.OnStaminaUpdate += HandleOnStaminaUpdated;
            _slider = GetComponent<Slider>();
        }

        private void HandleOnStaminaUpdated(float stamina)
        {
            _slider.value = (float)stamina / _playerStamina.MaxStamina;
        }
    }
}
