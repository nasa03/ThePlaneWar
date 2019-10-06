using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] UILabel roomLabel;
    [SerializeField] UILabel playersLabel;
    [SerializeField] UIToggle openToggle;
    [SerializeField] UISprite[] usernameSprides;
    [SerializeField] UIButton startGameButton;

    public void EnterOrRefreshRoom()
    {
        roomLabel.text = string.Format("房间名：{0}", PhotonNetwork.CurrentRoom.Name);
        playersLabel.text = string.Format("人数：{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
        openToggle.value = PhotonNetwork.CurrentRoom.IsVisible;
        openToggle.GetComponent<UIButton>().isEnabled = PhotonNetwork.LocalPlayer.IsMasterClient;

        Player localPlayer = PhotonNetwork.LocalPlayer;
        usernameSprides[0].GetComponentInChildren<UILabel>().text = localPlayer.NickName;

        if (localPlayer.IsMasterClient)
        {
            startGameButton.GetComponentInChildren<UILabel>().text = "开始游戏";
            usernameSprides[0].GetComponentInChildren<UILabel>().color = Color.red;
        }
        else
        {
            startGameButton.GetComponentInChildren<UILabel>().text = "准备";
            usernameSprides[0].GetComponentInChildren<UILabel>().color = Color.white;
            CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "isReady", false);
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
            {
                if ((bool)CustomProperties.GetProperties(player, "isReady", false))
                    usernameSprides[i + 1].GetComponentInChildren<UILabel>().color = Color.yellow;
                else
                    usernameSprides[i + 1].GetComponentInChildren<UILabel>().color = Color.white;
            }
        }
    }

    public void OpenToggleValueChanged()
    {
        PhotonNetwork.CurrentRoom.IsVisible = openToggle.value;
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
        if (startGameButton.transform.Find("Label").GetComponent<UILabel>().text == "开始游戏")
        {
            bool isAllReady = true;
            for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
            {
                if (!(bool)CustomProperties.GetProperties(PhotonNetwork.PlayerListOthers[i], "isReady", false))
                {
                    isAllReady = false;
                    break;
                }
            }
            if (isAllReady)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;

                PhotonNetwork.LoadLevel(1);
                startGameButton.isEnabled = false;
            }
            else
            {
                StartCoroutine(FindObjectOfType<MessageShow>().Show("玩家还没准备好"));
            }
        }
        else
        {
            if (startGameButton.transform.Find("Label").GetComponent<UILabel>().text == "准备")
            {
                startGameButton.GetComponentInChildren<UILabel>().text = "取消准备";
                usernameSprides[0].GetComponentInChildren<UILabel>().color = Color.yellow;
                CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "isReady", true);
            }
            else
            {
                startGameButton.GetComponentInChildren<UILabel>().text = "准备";
                usernameSprides[0].GetComponentInChildren<UILabel>().color = Color.white;
                CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "isReady", false);
            }
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        EnterOrRefreshRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        EnterOrRefreshRoom();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        openToggle.value = PhotonNetwork.CurrentRoom.IsOpen;
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (target == PhotonNetwork.PlayerListOthers[i])
            {
                if ((bool)CustomProperties.GetProperties(target, "isReady", false))
                    usernameSprides[i + 1].GetComponentInChildren<UILabel>().color = Color.yellow;
                else
                    usernameSprides[i + 1].GetComponentInChildren<UILabel>().color = Color.white;
            }
        }
    }
}
