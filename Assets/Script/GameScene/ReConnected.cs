using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class ReConnected : MonoBehaviourPunCallbacks
{
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Global.returnState = Global.ReturnState.Disconnected;

        PhotonNetwork.ReconnectAndRejoin();
        FindObjectOfType<PhotonGame>().Disconnect();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Global.returnState = Global.ReturnState.Normal;
        
        FindObjectOfType<PhotonGame>().RebornEnd();
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
