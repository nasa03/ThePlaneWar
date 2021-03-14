using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Vehicles.Aeroplane;

public class AIPositionOverlap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI") && PhotonNetwork.IsMasterClient)
        {
            other.GetComponent<BehaviorTree>().SetVariableValue("target", null);
            other.GetComponent<AeroplaneAiControl>().SetTarget(null);
        }
    }
}