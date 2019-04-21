using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PhotonGame : MonoBehaviourPunCallbacks {
    [SerializeField] GameObject[] planePrefabs;
    [SerializeField] Transform[] groundRunwayPosotion;
    GameObject localPlane;
    
    // Start is called before the first frame update
    void Start()
    {
        int planeInt = Global.totalPlaneInt;
        int playerInt = Global.totalPlayerInt;
        localPlane = PhotonNetwork.Instantiate(planePrefabs[planeInt].name, groundRunwayPosotion[playerInt].position + new Vector3(0, 10, 0), Quaternion.identity);

        Global.inGame = true;
    }

    public void OnExitButtonClick()
    {
        PhotonNetwork.Destroy(localPlane);
        SceneManager.LoadScene("StartScene");
    }
}
