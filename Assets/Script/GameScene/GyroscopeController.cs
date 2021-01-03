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
    [SerializeField] private MobileInputController mobileInputController;

    // Start is called before the first frame update
    private void Start()
    {
        stickBackground.enabled = !Global.bGyroscopeEnabled;
        stickForeground.enabled = !Global.bGyroscopeEnabled;
        mobileInputController.enabled = !Global.bGyroscopeEnabled;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!Global.bGyroscopeEnabled) return;

        float horizontal = Mathf.Clamp(Input.acceleration.x * Global.gyroscopeMultiple, -1.0f, 1.0f);
        float vertical = Mathf.Clamp((-Input.acceleration.y - 0.5f) * Global.gyroscopeMultiple, -1.0f, 1.0f);
        
        CrossPlatformInputManager.SetAxis("Horizontal", horizontal);
        CrossPlatformInputManager.SetAxis("Vertical", vertical);
    }
}
