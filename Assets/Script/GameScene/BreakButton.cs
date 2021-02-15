using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class BreakButton : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject button;
    private BreakType _breakType = BreakType.Normal;
    private float _time = 0.0f;
    private const int CoolingMaxTime = 10;

    private enum BreakType
    {
        Normal,
        Cooling
    }

    // Start is called before the first frame update
    void Start()
    {
        CoolingStart();
    }

    // Update is called once per frame
    void Update()
    {
        int maxTime = _breakType switch
        {
            BreakType.Cooling => CoolingMaxTime,
            _ => 0
        };
        
        if (_time > 0)
        {
            image.fillAmount = _time / maxTime;
            _time -= Time.deltaTime;
        }
        else
        {
            switch (_breakType)
            {
                case BreakType.Cooling:
                    CoolingEnd();
                    break;
                case BreakType.Normal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void CoolingStart()
    {
        _breakType = BreakType.Cooling;
        _time = CoolingMaxTime;
        image.fillAmount = 1.0f;
        button.GetComponent<Button>().enabled = false;
        button.GetComponent<EventTrigger>().enabled = false;
        button.GetComponent<ButtonHandler>().SetUpState();
    }
    
    private void CoolingEnd()
    {
        _breakType = BreakType.Normal;
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<EventTrigger>().enabled = true;
    }
}