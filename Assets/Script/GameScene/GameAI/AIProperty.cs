using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AIProperty : MonoBehaviour
{
    string name = null;

    public void Initialize(string name)
    {
        Name = name;
        if (PhotonNetwork.IsMasterClient)
        {
            HP = 100;
            Kill = 0;
            Death = 0;
        }
    }

    public string Name
    {
        get => name;
        set => name = value;
    }

    public int HP
    {
        get => (int) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, name + "-HP");
        set => CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, name + "-HP", value);
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
