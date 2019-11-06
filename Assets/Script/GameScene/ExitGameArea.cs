using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExitGameArea : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Plane")
        {
            FindObjectOfType<PlaneAttack>().Suiside(other.GetComponent<PhotonView>().Controller);
        }
    }
}
