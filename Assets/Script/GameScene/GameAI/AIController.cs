using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Vehicles.Aeroplane;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    AeroplaneAiControl aeroplaneAiControl;
    Transform target;
    int random = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (!target)
        {
            Transform tempTarget = GetNearTargetTransform();
            if (tempTarget)
            {
                target = tempTarget;
                aeroplaneAiControl.SetTarget(target);
            }
            else
            {
                getRandomPosition();
            }
        }
    }

    private void Awake()
    {
        aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
    }

    Transform GetNearTargetTransform()
    {
        ArrayList missileTargets = new ArrayList();
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].GetComponent<PhotonView>().IsMine)
                missileTargets.Add(targets[i].transform);
        }

        Vector3 thisPosition = transform.position;
        float[] distances = new float[missileTargets.Count];
        for (int i = 0; i < missileTargets.Count; i++)
        {
            Vector3 targetPosition = (missileTargets[i] as Transform).position;
            Vector3 dir = targetPosition - thisPosition;
            float dot = Vector3.Dot(Vector3.forward, dir);
            if (dot > 0)
                distances[i] = Vector3.Distance(thisPosition, targetPosition);
            else
                distances[i] = 0;
        }

        float minDistance = 0;
        Transform minTarget = null;
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] != 0 && (minDistance == 0 || distances[i] < minDistance))
            {
                minDistance = distances[i];
                minTarget = missileTargets[i] as Transform;
            }
        }

        if (minDistance <= 500.0f && minTarget)
        {
            return minTarget;
        }
        else
        {
            return null;
        }
    }

   public void getRandomPosition()
    {
        int random = Random.Range(0, FindObjectOfType<PhotonGameAI>().RandomPositions.Length);
        target = FindObjectOfType<PhotonGameAI>().RandomPositions[random];
        aeroplaneAiControl.SetTarget(target);
    }
}
