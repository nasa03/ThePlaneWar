using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.CrossPlatformInput;

public class ExitGameArea : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plane") && other.GetComponent<PhotonView>().IsMine)
        {
            CrossPlatformInputManager.SetButtonDown("Suicide");
        }
    }
}
