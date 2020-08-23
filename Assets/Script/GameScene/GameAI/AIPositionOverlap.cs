using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AIPositionOverlap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI" && PhotonNetwork.IsMasterClient)
        {
            other.GetComponent<AIController>().getRandomPosition();
        }
    }
}
