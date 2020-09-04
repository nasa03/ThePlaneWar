using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ReConnected : MonoBehaviourPunCallbacks
{
    private string _roomName;
    private int _planeCount;

    // Start is called before the first frame update
    private void Start()
    {
        _roomName = PhotonNetwork.CurrentRoom.Name;
        _planeCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Global.returnState = Global.ReturnState.Disconnected;

        if (_planeCount == 1)
            SceneManager.LoadScene("StartScene");
        else
        {
            PhotonNetwork.Reconnect();
            FindObjectOfType<PhotonGame>().Disconnect();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        FindObjectOfType<PhotonGame>().RebornEndOrReconnect();

        Global.returnState = Global.ReturnState.Normal;
    }
}
