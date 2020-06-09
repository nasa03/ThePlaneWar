using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraActive : MonoBehaviour
{
    [SerializeField] GameObject camera;
    BinnacleScript binnacleScript;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            camera.SetActive(true);

            binnacleScript.player = transform;
            binnacleScript.playerCamera = camera.transform;
        }
    }

    private void Awake()
    {
        binnacleScript = FindObjectOfType<BinnacleScript>();
    }
}
