using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMode : MonoBehaviour
{
    [SerializeField] private UIToggle toggle;

    public void OnValueChange()
    {
        Global.isOffline = toggle.value;
    }
}
