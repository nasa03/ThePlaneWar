using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class PowerSpeedButton : MonoBehaviour
{
    enum SpeedType
    {
        normal, powerSpeed, cooling
    }

    [SerializeField] Image image;
    [SerializeField] GameObject button;
    SpeedType speedType = SpeedType.normal;
    float time = 0.0f;
    const int powerSpeedMaxTime = 5;
    const int coolingMaxTime = 30;

    // Update is called once per frame
    void Update()
    {
        int maxTime = 0;
        if (speedType == SpeedType.powerSpeed)
            maxTime = powerSpeedMaxTime;
        else if (speedType == SpeedType.cooling)
            maxTime = coolingMaxTime;

        if (time > 0)
        {
            image.fillAmount = time / maxTime;
            time -= Time.deltaTime;
        }
        else
        {
            if (speedType == SpeedType.powerSpeed)
            {
                CoolingStart();
            }
            else if (speedType == SpeedType.cooling)
            {
                PowerSpeedEnd();
            }
        }
    }

    public void PowerSpeedStart()
    {
        speedType = SpeedType.powerSpeed;
        time = powerSpeedMaxTime;
        image.fillAmount = 1.0f;
    }

    public void CoolingStart()
    {
        speedType = SpeedType.cooling;
        time = coolingMaxTime;
        image.fillAmount = 1.0f;
        button.GetComponent<Button>().enabled = false;
        button.GetComponent<EventTrigger>().enabled = false;
        button.GetComponent<ButtonHandler>().SetUpState();
    }

    void PowerSpeedEnd()
    {
        speedType = SpeedType.normal;
        button.GetComponent<Button>().enabled = true;
        button.GetComponent<EventTrigger>().enabled = true;
    }
}
