using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;
using UnityEngine.Serialization;

public class MissileActor : MonoBehaviourPun
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

        if (!CrossPlatformInputManager.GetButtonDown("Fire2") ||
            !FindObjectOfType<MissileActorButton>().Missile) return;

        if (cameraShake)
            GetComponent<projectileActor>().CameraShakeCaller.ShakeCamera();
        
        PhotonNetwork.Instantiate(missiles[missileType].name, transform.position, transform.rotation);
        FindObjectOfType<MissileActorButton>().ShootStart();
    }

    public void AIShoot()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject missile = PhotonNetwork.Instantiate(missiles[0].name, transform.position, transform.rotation);
        missile.AddComponent<AIBullet>().aiTarget = transform;
    }
}
