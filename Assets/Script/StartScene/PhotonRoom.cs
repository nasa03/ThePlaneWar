using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks {
    [SerializeField] UILabel roomLabel;
    [SerializeField] UILabel playersLabel;
    [SerializeField] UIToggle openToggle;
    [SerializeField] UISprite[] usernameSprides;
    [SerializeField] UIButton startGameButton;

    public void EnterRoom()
    {
        roomLabel.text = string.Format("房间名：{0}", PhotonNetwork.CurrentRoom.Name);
        playersLabel.text = string.Format("人数：{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
        openToggle.value = PhotonNetwork.CurrentRoom.IsOpen;
        openToggle.GetComponent<UIButton>().isEnabled = PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    public void Refresh()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        usernameSprides[0].GetComponentInChildren<UILabel>().text = localPlayer.NickName;

        if (localPlayer.IsMasterClient)
        {
            startGameButton.isEnabled = true;
            usernameSprides[0].GetComponentInChildren<UILabel>().color = Color.red;
        }
        else
        {
            startGameButton.isEnabled = false;
            usernameSprides[0].GetComponentInChildren<UILabel>().color = Color.white;
        }

        FindObjectOfType<ChoosePlane>().SetPlayerInt(localPlayer);

        FindObjectOfType<ShowPlane>().DestroyAll();
        for (int i = 1; i < 6; i++)
            usernameSprides[i].gameObject.SetActive(false);

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            Player player = PhotonNetwork.PlayerListOthers[i];

            usernameSprides[i + 1].gameObject.SetActive(true);
            usernameSprides[i + 1].GetComponentInChildren<UILabel>().text = player.NickName;

            FindObjectOfType<ShowPlane>().Show(i);

            if (player.IsMasterClient)
                usernameSprides[i + 1].GetComponentInChildren<UILabel>().color = Color.red;
            else
                usernameSprides[i + 1].GetComponentInChildren<UILabel>().color = Color.white;
        }
    }

    public void OpenToggleValueChanged()
    {
        PhotonNetwork.CurrentRoom.IsOpen = openToggle.value;
    }

    public void LeftRoomButtonOnClick()
    {
        if (!PhotonNetwork.OfflineMode)
            PhotonNetwork.LeaveRoom();
        else
            PhotonNetwork.Disconnect();
    }

    public void StartGameButtonOnClick()
    {
        PhotonNetwork.LoadLevel(1);
        startGameButton.isEnabled = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Refresh();
        EnterRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Refresh();
        EnterRoom();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        openToggle.value = PhotonNetwork.CurrentRoom.IsOpen;
    }
}
