using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class ChooseRoom : MonoBehaviour
{
    [HideInInspector] public RoomInfo roomInfo;

    public void Choose()
    {
        PhotonLobby lobby = FindObjectOfType<PhotonLobby>();

        lobby.join.joinWidget.SetActive(true);
        lobby.create.CreateWidget.SetActive(false);

        lobby.join.nameLabel.text = roomInfo.Name;
        lobby.join.playersLabel.text = string.Format("人数:{0}/{1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);

        if (roomInfo.IsOpen)
            lobby.join.openLabel.text = "开放";
        else
            lobby.join.openLabel.text = "锁定";

        lobby.join.totalRoonInfo = roomInfo;
    }
}
