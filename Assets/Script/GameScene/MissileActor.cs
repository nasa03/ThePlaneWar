using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;

public class MissileActor : MonoBehaviourPun
{
    [SerializeField] GameObject[] missiles;
    [SerializeField] float interval = 10;
    [SerializeField] int missileType = 0;

    // Use this for initialization
    void Start()
    {
        missileType = Global.totalMissileInt;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (CrossPlatformInputManager.GetButtonDown("Fire2") && FindObjectOfType<MissileActorButton>().Missile)
        {
            GetComponent<projectileActor>().CameraShakeCaller.ShakeCamera();
            PhotonNetwork.Instantiate(missiles[missileType].name, transform.position, transform.rotation);
            FindObjectOfType<MissileActorButton>().ShootStart();
        }
    }
}
