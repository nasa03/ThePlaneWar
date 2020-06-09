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
            StartCoroutine(FindObjectOfType<PlaneAttack>().Suicide(GetComponent<PhotonView>().Controller));
        }
    }
}
