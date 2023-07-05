using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetableCanvasHandler : MonoBehaviour
{
    [SerializeField] Image _targetImage;
    private bool _isActive = false;
    private void Start()
    {
        _targetImage.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (_isActive)
            transform.rotation = Camera.main.transform.rotation;
    }
    public void SetActive(bool activeState)
    {
        _targetImage.gameObject.SetActive(activeState);
        _isActive = activeState;
    }
}
