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

    public Global.AIPlaneScores aiPlaneScores => new Global.AIPlaneScores
    {
        name = gameObject.name,
        kill = Kill,
        death = Death
    };

    public int HP
    {
        get => (int) PhotonNetwork.CurrentRoom.GetProperties(name + "-HP", 100);
        set => PhotonNetwork.CurrentRoom.SetProperties(name + "-HP", value);
    }

    public int Kill
    {
        get => (int) PhotonNetwork.CurrentRoom.GetProperties(name + "-kill", 0);
        set => PhotonNetwork.CurrentRoom.SetProperties(name + "-kill", value);
    }

    public int Death
    {
        get => (int) PhotonNetwork.CurrentRoom.GetProperties(name + "-death", 0);
        set => PhotonNetwork.CurrentRoom.SetProperties(name + "-death", value);
    }
}
