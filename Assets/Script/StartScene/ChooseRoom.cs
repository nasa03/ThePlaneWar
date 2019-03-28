using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChooseRoom : MonoBehaviour
{
    [HideInInspector] public RoomInfo roomInfo;

    public void Join()
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }
}
