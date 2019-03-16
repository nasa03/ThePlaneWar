using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExitGame : MonoBehaviour
{
    public void OnClick()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
