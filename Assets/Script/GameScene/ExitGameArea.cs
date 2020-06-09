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
            StartCoroutine(FindObjectOfType<PlaneAttack>().Suicide(other.GetComponent<PhotonView>().Controller));
        }
    }
}
