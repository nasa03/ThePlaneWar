using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class ReConnected : MonoBehaviourPunCallbacks
{
    private string _roomName;

    // Start is called before the first frame update
    private void Start()
    {
        _roomName = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Global.returnState = Global.ReturnState.Disconnected;

        PhotonNetwork.Reconnect();
        FindObjectOfType<PhotonGame>().Disconnect();
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        FindObjectOfType<PhotonGame>().photonView.RPC("LittleHeathBarReload", RpcTarget.All, false, null);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        
        FindObjectOfType<PhotonGame>().photonView.RPC("LittleHeathBarReload", RpcTarget.All, false, null);
    }
}
