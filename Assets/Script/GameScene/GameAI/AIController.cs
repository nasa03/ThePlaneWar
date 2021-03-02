using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        Transform tempTarget = Global.GetNearTargetTransform(transform);
        
        if (tempTarget && GetDistance(tempTarget) < 1000.0f)
            Target = tempTarget;
        else
            Target = GetRandomPosition();
        
        _aeroplaneAiControl.SetTarget(Target);
    }

    private void Awake()
    {
        _aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
    }

    private Transform GetRandomPosition()
    {
        int random = -1;

        do random = Random.Range(0, FindObjectOfType<PhotonGameAI>().RandomPositions.Length);
        while (random == _random);

        _random = random;
        return FindObjectOfType<PhotonGameAI>().RandomPositions[random];
    }

    private float GetDistance(Transform target)
    {
        float distance = 0;
        
        if (Target.CompareTag("Plane") && Target.CompareTag("AI"))
        {
            Vector3 thisPosition = transform.position;
            Vector3 targetPosition = target.position;
            Vector3 dir = targetPosition - thisPosition;
            float dot = Vector3.Dot(Vector3.forward, dir);
            if (dot > 0)
                distance = Vector3.Distance(thisPosition, targetPosition);
        }

        return distance;
    }
}