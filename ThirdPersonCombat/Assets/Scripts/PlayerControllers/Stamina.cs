using Inputs;
using System;
using System.Collections;
using UnityEngine;

namespace PlayerController
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] float _maxStamina = 200;
        [SerializeField] float _regenerateSpeedOnMovement = 1.5f;
        [SerializeField] float _regenerateSpeed = 1.5f;
        private float _stamina = 0f;
        private bool _isFull = true;
        private InputReader _inputReader;
        public bool stopRegenerate;
        public float MaxStamina { get => _maxStamina;}
        private bool _isStaminaUsing;

        public event System.Action<float> OnStaminaUpdate;

        private void Awake()
        {
            _inputReader = GetComponent<InputReader>();
        }
        private void OnEnable()
        {
            _stamina = MaxStamina;
        }
        public bool UseStamina(float value)
        {
            //Allow player to spend more stamina than required stamina if stamina is too low, one time.
            if (_stamina < value)
            {
                if (_stamina >= (value / _maxStamina) * 45f) // 45 for avarage stamina cost values
                {
                    _stamina = 0f;
                    _isStaminaUsing = true;
                    StopAllCoroutines();
                    StartCoroutine(StaminaUsedCooldown());
                    OnStaminaUpdate?.Invoke(_stamina);
                    return true;
                }
                return false;
            }
            _stamina -= value;
            _isFull = false;
            OnStaminaUpdate?.Invoke(_stamina);
            _isStaminaUsing = true;
            StopAllCoroutines();
            StartCoroutine(StaminaUsedCooldown());
            return true;
        }
        private void Update()
        {
            if (_isFull || _isStaminaUsing) return;
            if (stopRegenerate) return;
            if(_inputReader.MovementOn2DAxis.magnitude < 0.1f)
            {
                _stamina += Time.deltaTime * _regenerateSpeed;
            }
            else
            {
                _stamina += Time.deltaTime * _regenerateSpeedOnMovement;
            }
            if(_stamina >= MaxStamina)
            {
                _stamina = MaxStamina;
                _isFull = true;
            }

            OnStaminaUpdate?.Invoke(_stamina);
        }

        private WaitForSeconds _staminaCooldown = new WaitForSeconds(0.1f);
        IEnumerator StaminaUsedCooldown()
        {
            yield return _staminaCooldown;
            _isStaminaUsing = false;
            yield return null;
        }

        internal void ResetStamina()
        {
            _stamina = MaxStamina;
            OnStaminaUpdate?.Invoke(_stamina);
        }
    }
}

