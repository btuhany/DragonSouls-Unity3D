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
        public float MaxStamina { get => _maxStamina;}

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
            if (_stamina < value) return false;
            _stamina -= value;
            _isFull = false;
            return true;
        }
        private void Update()
        {
            if (_isFull) return;
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
    }
}

