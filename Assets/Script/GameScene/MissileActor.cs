using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;

public class MissileActor : MonoBehaviourPun, IShootActor
{
    [SerializeField] private GameObject[] missiles;
    [SerializeField] private float interval = 10;
    [SerializeField] private int missileType = 0;
    [SerializeField] private bool cameraShake = true;

    // Use this for initialization
    private void Start()
    {
        missileType = Global.totalMissileInt;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!photonView.IsMine || CompareTag("AI")) return;

        if (!FindObjectOfType<MissileActorButton>().Missile) return;

        if (CrossPlatformInputManager.GetButtonDown("Fire2")) Fire();
    }

    public void Fire()
    {
        if (cameraShake)
            GetComponent<projectileActor>().CameraShakeCaller.ShakeCamera();

        PhotonNetwork.Instantiate(missiles[missileType].name, transform.position, transform.rotation);
        FindObjectOfType<MissileActorButton>().ShootStart();
    }

    public void AIShoot()
    {
        GameObject missile = PhotonNetwork.Instantiate(missiles[0].name, transform.position, transform.rotation);
        missile.AddComponent<AIBullet>().aiTarget = transform;
    }
}