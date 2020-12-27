using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField] private Image stickBackground;
    [SerializeField] private Image stickForeground;
    [SerializeField] private Text stickText;
    [SerializeField] private MobileInputController mobileInputController;
    private bool _isEnabled = false;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!_isEnabled) return;

        float horizontal = Mathf.Clamp(Input.acceleration.x, -1.0f, 1.0f);
        float vertical = Mathf.Clamp(-Input.acceleration.y - 0.5f, -1.0f, 1.0f);
        
        CrossPlatformInputManager.SetAxis("Horizontal", horizontal);
        CrossPlatformInputManager.SetAxis("Vertical", vertical);
    }

    public void Turn()
    {
        _isEnabled = !_isEnabled;
        stickBackground.enabled = !_isEnabled;
        stickForeground.enabled = !_isEnabled;
        mobileInputController.enabled = !_isEnabled;
        stickText.text = _isEnabled ? "陀" : "摇";
    }
}
