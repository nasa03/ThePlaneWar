using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlaneCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "FX")
        {
            FindObjectOfType<PlaneAttack>().Suiside(GetComponent<PhotonView>().Controller);
        }
    }
}
