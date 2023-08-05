using Combat;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUI
{
    public class PlayerUIHealthBar : MonoBehaviour
    {
        [SerializeField] private Health _playerHealth;
        private Slider _slider;

        private void Awake()
        {
            _playerHealth.OnHealthUpdated += HandleOnHealthUpdated;
            _slider = GetComponent<Slider>();
        }

        private void HandleOnHealthUpdated(int health, int damage)
        {
            _slider.value = (float)health / _playerHealth.maxHealth;
        }
    }

}
