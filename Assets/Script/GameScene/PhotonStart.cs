﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonStart : MonoBehaviour
{
    [SerializeField] GameObject[] planePrefabs;
    [SerializeField] Transform[] groundRunwayPosotion;
    
    // Start is called before the first frame update
    void Start()
    {
        int planeInt = FindObjectOfType<ChoosePlane>().TotalPlaneInt;
        PhotonNetwork.Instantiate(planePrefabs[planeInt].name, groundRunwayPosotion[Random.Range(0, groundRunwayPosotion.Length)].position + new Vector3(0, 10, 0), Quaternion.identity);
    }
}
