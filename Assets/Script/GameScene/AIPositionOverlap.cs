using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPositionOverlap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Plane")
        {
            other.GetComponent<PlaneAI>().getRandomPosition();
        }
    }
}
