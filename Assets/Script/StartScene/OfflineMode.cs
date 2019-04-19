using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OfflineMode : MonoBehaviour
{
    [SerializeField] UIToggle toggle;
    bool isOffline = false;
    public bool IsOffline { get { return isOffline;} }

    public void OnValueChange()
    {
        isOffline = toggle.value;
    }
}
