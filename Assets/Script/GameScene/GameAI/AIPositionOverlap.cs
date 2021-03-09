using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Photon.Pun;

public class AIPositionOverlap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI") && PhotonNetwork.IsMasterClient)
            other.GetComponent<BehaviorTree>().ExternalBehavior.SetVariable("target", null);
    }
}