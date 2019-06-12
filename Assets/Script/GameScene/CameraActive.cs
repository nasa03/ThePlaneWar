using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraActive : MonoBehaviour
{
    [SerializeField] GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            camera.SetActive(true);

            FindObjectOfType<BinnacleScript>().player = transform;
            FindObjectOfType<BinnacleScript>().playerCamera = camera.transform;
        }
    }
}
