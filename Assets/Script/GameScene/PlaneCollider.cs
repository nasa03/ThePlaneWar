using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "FX" || collision.collider.tag == "Plane")
        {
            FindObjectOfType<PlaneAttack>().Suiside();
        }
    }
}
