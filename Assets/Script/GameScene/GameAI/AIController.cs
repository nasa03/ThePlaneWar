using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class AIController : MonoBehaviour
{
    private projectileActor _projectileActor;
    private MissileActor _missileActor;
    private float _missileTime = MAXMissileTime;
    private int _missileCount = MAXMissileCount;
    private const float MAXMissileTime = 10.0f;
    private const int MAXMissileCount = 3;

    private void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        //if (Target)
        //{
        //    if (GetDistance(Target) == 0) return;
        //    
        //    if (GetDistance(Target) < 500.0f)
        //        _projectileActor.AIShoot();
//
        //    if (_missileCount > 0)
        //    {
        //        _missileCount--;
        //        _missileActor.AIShoot();
        //    }
//
        //    return;
        //};
        
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
        _projectileActor = GetComponent<projectileActor>();
        _missileActor = GetComponent<MissileActor>();
    }
}