using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class PowerSpeedButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject button;
    private ButtonType _buttonType = ButtonType.Normal;
    private float _time = 0.0f;
    private const int ProcessingMaxTime = 10;
    private const int CoolingMaxTime = 15;

    private enum ButtonType
    {
        Normal,
        Processing,
        Cooling
    }

    // Update is called once per frame
    private void Update()
    {
        int maxTime = _buttonType switch
        {
            ButtonType.Processing => ProcessingMaxTime,
            ButtonType.Cooling => CoolingMaxTime,
            _ => 0
        };

        if (_time > 0)
        {
            image.fillAmount = _time / maxTime;
            _time -= Time.deltaTime;
        }
        else
        {
            switch (_buttonType)
            {
                case ButtonType.Processing:
                    CoolingStart();
                    break;
                case ButtonType.Cooling:
                    CoolingEnd();
                    break;
                case ButtonType.Normal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void ProcessingStart()
    {
        _buttonType = ButtonType.Processing;
        _time = ProcessingMaxTime;
        image.fillAmount = 1.0f;
    }

    public void CoolingStart()
    {
        _buttonType = ButtonType.Cooling;
        _time = CoolingMaxTime;
        image.fillAmount = 1.0f;
        button.GetComponent<Button>().enabled = false;
        button.GetComponent<EventTrigger>().enabled = false;
        button.GetComponent<ButtonHandler>().SetUpState();
    }

    private void CoolingEnd()
    {
        _buttonType = ButtonType.Normal;
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<EventTrigger>().enabled = true;
    }
}