using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Vehicles.Aeroplane;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private AeroplaneAiControl _aeroplaneAiControl;
    private int _random = -1;

    public Transform Target { private get; set; }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (Target) return;

        Transform tempTarget = GetNearTargetTransform();
        if (!tempTarget)
            tempTarget = GetRandomPosition();

        Target = tempTarget;
        _aeroplaneAiControl.SetTarget(Target);
    }

    private void Awake()
    {
        _aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
    }

    private Transform GetNearTargetTransform()
    {
        ArrayList list = new ArrayList();

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject target in targets)
        {
            list.Add(target.transform);
        }

        GameObject[] aiTargets = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject target in aiTargets)
        {
            if (target != gameObject)
            {
                list.Add(target.transform);
            }
        }

        Vector3 thisPosition = transform.position;
        float[] distances = new float[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 targetPosition = (list[i] as Transform).position;
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
            if (distances[i] == 0 || (minDistance != 0 && !(distances[i] < minDistance))) continue;

            minDistance = distances[i];
            minTarget = list[i] as Transform;
        }

        if (minDistance <= 500.0f && minTarget)
        {
            return minTarget;
        }

        return null;
    }

    private Transform GetRandomPosition()
    {
        int random = -1;
        do
        {
            random = Random.Range(0, FindObjectOfType<PhotonGameAI>().RandomPositions.Length);
        } while (random == _random);

        _random = random;
        return FindObjectOfType<PhotonGameAI>().RandomPositions[random];
    }
}