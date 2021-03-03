using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Vehicles.Aeroplane;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private AeroplaneAiControl _aeroplaneAiControl;
    private projectileActor _projectileActor;
    private MissileActor _missileActor;
    private int _random = -1;
    private float _missileTime = MAXMissileTime;
    private int _missileCount = MAXMissileCount;
    private const float MAXMissileTime = 10.0f;
    private const int MAXMissileCount = 3;

    public Transform Target { private get; set; }
    
    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (Target)
        {
            if (GetDistance(Target) == 0) return;
            
            if (GetDistance(Target) < 500.0f)
                _projectileActor.AIShoot();

            if (_missileCount > 0)
                _missileActor.AIShoot();

            return;
        };

        Transform tempTarget = Global.GetNearTargetTransform(transform);

        if (tempTarget && GetDistance(tempTarget) < 1000.0f && GetDistance(tempTarget) != 0)
            Target = tempTarget;
        else
            Target = GetRandomPosition();
        
        _aeroplaneAiControl.SetTarget(Target);
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (_missileTime > 0 && _missileCount < MAXMissileCount)
            _missileTime -= Time.deltaTime;
        else
        {
            if (_missileCount < MAXMissileCount)
                _missileCount++;

            _missileTime = MAXMissileTime;
        }
    }

    private void Awake()
    {
        _aeroplaneAiControl = GetComponent<AeroplaneAiControl>();
        _projectileActor = GetComponent<projectileActor>();
        _missileActor = GetComponent<MissileActor>();
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