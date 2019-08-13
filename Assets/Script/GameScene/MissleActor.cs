using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;

public class MissleActor : MonoBehaviour
{
    [SerializeField] GameObject[] missiles;
    [SerializeField] float interval = 10;
    [SerializeField] int missileType = 0;

    // Use this for initialization
    void Start()
    {
        missileType = Global.totalMissleInt;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        if (CrossPlatformInputManager.GetButtonDown("Fire2"))
        {
            GetComponent<projectileActor>().CameraShakeCaller.ShakeCamera();
            Fire();
        }
    }

    void Fire()
    {
        PhotonNetwork.Instantiate(missiles[missileType].name, transform.position, transform.rotation);
    }
}
