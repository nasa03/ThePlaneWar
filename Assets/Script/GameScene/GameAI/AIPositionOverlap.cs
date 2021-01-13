using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AIPositionOverlap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AI") && PhotonNetwork.IsMasterClient)
            other.GetComponent<AIController>().Target = null;
    }
}
