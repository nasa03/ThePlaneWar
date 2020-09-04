using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Vehicles.Aeroplane;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private AeroplaneAiControl _aeroplaneAiControl;
    private Transform _target;
    private int _random = 0;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_target) return;
        
        Transform tempTarget = GetNearTargetTransform();
        if (tempTarget)
        {
            _target = tempTarget;
            _aeroplaneAiControl.SetTarget(_target);
        }
        else
        {
            GetRandomPosition();
        }
    }

    private void Awake()
    {
        _aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
    }

    private Transform GetNearTargetTransform()
    {
        ArrayList missileTargets = new ArrayList();
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");

        foreach (var target in targets)
        {
            if (!target.GetComponent<PhotonView>().IsMine)
                missileTargets.Add(target.transform);
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

    public void GetRandomPosition()
    {
        int random = Random.Range(0, FindObjectOfType<PhotonGameAI>().RandomPositions.Length);
        _target = FindObjectOfType<PhotonGameAI>().RandomPositions[random];
        _aeroplaneAiControl.SetTarget(_target);
    }
}
