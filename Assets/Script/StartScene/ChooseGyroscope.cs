using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseGyroscope : MonoBehaviour
{
    [SerializeField] private UIToggle gyroscopeCheckBox;
    [SerializeField] private UISlider gyroscopeMultipleSlider;

    public void OnGyroscopeValueChange()
    {
        Global.bGyroscopeEnabled = gyroscopeCheckBox.value;
    }
    
    public void OnMultipleValueChange()
    {
        Global.gyroscopeMultiple = gyroscopeMultipleSlider.value * 2;
    }
}
