using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class ChooseRoom : SingletonManager<ChooseRoom>
{
    [SerializeField] UILabel nameLabel;
    [SerializeField] UILabel playersLabel;
    [SerializeField] UILabel openLabel;
    [HideInInspector] public RoomInfo roomInfo;

    public void Show()
    {
        nameLabel.text = roomInfo.Name;
        playersLabel.text = string.Format("人数:{0}/{1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);

        if (roomInfo.IsOpen)
            openLabel.text = "开放";
        else
            openLabel.text = "锁定";
    }
}
