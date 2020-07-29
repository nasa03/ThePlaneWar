using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AIScore : MonoBehaviour
{
    string name = null;

    public void Initialize(string name)
    {
        Name = name;
        if (PhotonNetwork.IsMasterClient)
        {
            Kill = 0;
            Death = 0;
        }
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public int Kill
    {
        get => (int) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, name + "-kill");
        set => CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, name + "-kill", value);
    }

    public int Death
    {
        get => (int) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, name + "-death");
        set => CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, name + "-death", value);
    }
}
