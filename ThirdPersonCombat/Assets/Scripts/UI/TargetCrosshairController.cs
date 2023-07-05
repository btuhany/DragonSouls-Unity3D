using UnityEngine;
using UnityEngine.UI;

namespace UIControllers
{
    public class TargetCrosshairController : MonoBehaviour
    {
        private Transform _currentTargetTransform;
        private Image _targetCrosshairImage;
        private void Awake()
        {
            _targetCrosshairImage = GetComponent<Image>();    
        }
        private void Start()
        {
            _targetCrosshairImage.enabled = false;
        }
        private void Update()
        {
            if (_currentTargetTransform == null) return;
            this.transform.position = Camera.main.WorldToScreenPoint(_currentTargetTransform.position);
        }
        public void SetTargetState(Transform targetTransform)
        {
            _targetCrosshairImage.enabled = true;
            _currentTargetTransform = targetTransform;
        }
        public void ClearTarget()
        {
            _targetCrosshairImage.enabled = false;
            _currentTargetTransform = null;
        }
    }
}
