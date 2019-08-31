using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PhotonGame : MonoBehaviour {
    [SerializeField] GameObject[] planePrefabs;
    [SerializeField] Transform[] groundRunwayPosotion;
    GameObject localPlane;
    
    // Start is called before the first frame update
    void Start()
    {
        localPlane = PhotonNetwork.Instantiate(planePrefabs[Global.totalPlaneInt].name, groundRunwayPosotion[Global.totalPlayerInt].position + new Vector3(0, 10, 0), Quaternion.identity);

        Global.inGame = true;
    }

    public void OnExitButtonClick()
    {
        PhotonNetwork.Destroy(localPlane);
        SceneManager.LoadScene("StartScene");
    }

    public void Dead()
    {
        PhotonNetwork.Destroy(localPlane);
        localPlane = PhotonNetwork.Instantiate(planePrefabs[Global.totalPlaneInt].name, groundRunwayPosotion[Global.totalPlayerInt].position + new Vector3(0, 10, 0), Quaternion.identity);

        int death = (int)CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "death");
        death++;
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "death", death);
    }
}
