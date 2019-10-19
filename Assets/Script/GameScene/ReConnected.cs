using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ReConnected : MonoBehaviourPunCallbacks
{
    string roomName;
    int planeCount;

    // Start is called before the first frame update
    void Start()
    {
        roomName = PhotonNetwork.CurrentRoom.Name;
        planeCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Global.returnState = Global.ReturnState.disconnected;

        if (planeCount == 1)
            SceneManager.LoadScene("StartScene");
        else
        {
            PhotonNetwork.Reconnect();
            FindObjectOfType<PhotonGame>().Disonnect();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        FindObjectOfType<PhotonGame>().RebornEndOrReconnect();

        Global.returnState = Global.ReturnState.normal;
    }
}
