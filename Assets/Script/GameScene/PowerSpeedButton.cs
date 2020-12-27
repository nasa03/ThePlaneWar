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
    private SpeedType _speedType = SpeedType.Normal;
    private float _time = 0.0f;
    private const int PowerSpeedMaxTime = 10;
    private const int CoolingMaxTime = 30;

    private enum SpeedType
    {
        Normal, PowerSpeed, Cooling
    }

    // Start is called before the first frame update
    private void Start()
    {
        CoolingStart();
    }

    // Update is called once per frame
    private void Update()
    {
        int maxTime = _speedType switch
        {
            SpeedType.PowerSpeed => PowerSpeedMaxTime,
            SpeedType.Cooling => CoolingMaxTime,
            _ => 0
        };

        if (_time > 0)
        {
            image.fillAmount = _time / maxTime;
            _time -= Time.deltaTime;
        }
        else
        {
            switch (_speedType)
            {
                case SpeedType.PowerSpeed:
                    CoolingStart();
                    break;
                case SpeedType.Cooling:
                    PowerSpeedEnd();
                    break;
                case SpeedType.Normal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void PowerSpeedStart()
    {
        _speedType = SpeedType.PowerSpeed;
        _time = PowerSpeedMaxTime;
        image.fillAmount = 1.0f;
    }

    private void CoolingStart()
    {
        _speedType = SpeedType.Cooling;
        _time = CoolingMaxTime;
        image.fillAmount = 1.0f;
        button.GetComponent<Button>().enabled = false;
        button.GetComponent<EventTrigger>().enabled = false;
        button.GetComponent<ButtonHandler>().SetUpState();
    }

    private void PowerSpeedEnd()
    {
        _speedType = SpeedType.Normal;
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<EventTrigger>().enabled = true;
    }
}
