using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OfflineMode : MonoBehaviour
{
    [SerializeField] UIToggle toggle;

    public void OnValueChange()
    {
        Global.isOffline = toggle.value;
    }
}
