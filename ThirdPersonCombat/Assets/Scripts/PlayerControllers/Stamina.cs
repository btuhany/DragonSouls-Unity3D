using Inputs;
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

        public event System.Action<float> OnStaminaUpdate;

        private bool _isOneTimeLowStaminaUsed;
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
            if(_stamina > _maxStamina / 5)
            {
                if (_stamina < value && !_isOneTimeLowStaminaUsed)
                    return false;
            }
            else
            {
                if (_isOneTimeLowStaminaUsed)
                    return false;
                else
                    _isOneTimeLowStaminaUsed = true;
            }
            _stamina -= value;
            _isFull = false;
            OnStaminaUpdate?.Invoke(_stamina);
            return true;
        }
        private void Update()
        {
            if (_isFull) return;
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
            if (_isOneTimeLowStaminaUsed && _stamina > _maxStamina / 2.5f)
                _isOneTimeLowStaminaUsed = false;
            OnStaminaUpdate?.Invoke(_stamina);
        }
    }
}

