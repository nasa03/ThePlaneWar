using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AIProperty : MonoBehaviour
{
    public void Initialize(string name)
    {
        gameObject.name = name;
        HP = 100;
        Kill = 0;
        Death = 0;
    }

    public int HP
    {
        get => (int) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, name + "-HP", 100);
        set => CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, name + "-HP", value);
    }

    public int Kill
    {
        get => (int) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, name + "-kill", 0);
        set => CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, name + "-kill", value);
    }

    public int Death
    {
        get => (int) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, name + "-death", 0);
        set => CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, name + "-death", value);
    }
}
