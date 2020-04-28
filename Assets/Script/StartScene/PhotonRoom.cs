using System;
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
    [SerializeField] UIButton startGameButton;
    
    public UISprite[] usernameSprides;

    [PunRPC]
    public void EnterOrRefreshRoom()
    {
        roomLabel.text = string.Format("房间名：{0}", PhotonNetwork.CurrentRoom.Name);
        playersLabel.text = string.Format("人数：{0}/{1}",
            PhotonNetwork.CurrentRoom.PlayerCount + GetComponent<PhotonAI>().AI_List.Count,
            PhotonNetwork.CurrentRoom.MaxPlayers);
        openToggle.value = PhotonNetwork.CurrentRoom.IsVisible;
        openToggle.GetComponent<UIButton>().isEnabled = PhotonNetwork.LocalPlayer.IsMasterClient;

        Player localPlayer = PhotonNetwork.LocalPlayer;
        usernameSprides[0].GetComponentInChildren<UILabel>().text = localPlayer.NickName;

        if (localPlayer.IsMasterClient)
        {
            startGameButton.GetComponentInChildren<UILabel>().text = "开始游戏";
            usernameSprides[0].transform.Find("Name Label").GetComponent<UILabel>().color = Color.red;
        }
        else
        {
            startGameButton.GetComponentInChildren<UILabel>().text = "准备";
            usernameSprides[0].transform.Find("Name Label").GetComponent<UILabel>().color = Color.white;
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

            if (localPlayer.IsMasterClient)
            {
                usernameSprides[i + 1].transform.Find("Master Label").gameObject.SetActive(true);
                usernameSprides[i + 1].transform.Find("Kick Label").gameObject.SetActive(true);
            }
            else
            {
                usernameSprides[i + 1].transform.Find("Master Label").gameObject.SetActive(false);
                usernameSprides[i + 1].transform.Find("Kick Label").gameObject.SetActive(false);
            }

            FindObjectOfType<ShowPlane>().Show(i);

            if (player.IsMasterClient)
                usernameSprides[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.red;
            else
            {
                if ((bool)CustomProperties.GetProperties(player, "isReady", false))
                    usernameSprides[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.yellow;
                else
                    usernameSprides[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.white;
            }
        }
    }

    public void OpenToggleValueChanged()
    {
        PhotonNetwork.CurrentRoom.IsVisible = openToggle.value;
    }

    public void KickLabelOnClick(UILabel name)
    {
        for(int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (PhotonNetwork.PlayerListOthers[i].NickName == name.text)
            {
                photonView.RPC("KickPlayer", PhotonNetwork.PlayerListOthers[i]);
            }
        }
    }

    public void MasterLabelOnClick(UILabel name)
    {
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (PhotonNetwork.PlayerListOthers[i].NickName == name.text)
            {
                PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.PlayerListOthers[i]);
            }
        }
        
        photonView.RPC("EnterOrRefreshRoom", RpcTarget.All);
        photonView.RPC("EnterOrRefreshRoomOfAI", RpcTarget.All);
    }

    [PunRPC]
    public void KickPlayer()
    {
        PhotonNetwork.LeaveRoom();
        StartCoroutine(FindObjectOfType<MessageShow>().Show("已被踢出！"));
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

                PhotonNetwork.LoadLevel(FindObjectOfType<ChooseMap>().Index);
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
        
        FindObjectOfType<PhotonAI>().EnterOrRefreshRoomOfAI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        EnterOrRefreshRoom();
        
        FindObjectOfType<PhotonAI>().EnterOrRefreshRoomOfAI();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        openToggle.value = PhotonNetwork.CurrentRoom.IsVisible;
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
