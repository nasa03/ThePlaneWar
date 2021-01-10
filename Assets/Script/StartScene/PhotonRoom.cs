using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private UILabel roomLabel;
    [SerializeField] private UILabel playersLabel;
    [SerializeField] private UIToggle openToggle;
    [SerializeField] private UIButton startGameButton;
    [SerializeField] private UISprite[] usernameSprites;

    [PunRPC]
    public void EnterOrRefreshRoom()
    {
        roomLabel.text = $"房间名：{PhotonNetwork.CurrentRoom.Name}";
        playersLabel.text =
            $"人数：{PhotonNetwork.CurrentRoom.PlayerCount + GetComponent<PhotonAI>().AIList.Count}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        openToggle.value = PhotonNetwork.CurrentRoom.IsVisible;
        openToggle.GetComponent<UIButton>().isEnabled = PhotonNetwork.LocalPlayer.IsMasterClient;

        PhotonNetwork.CurrentRoom.IsVisible = true;

        Player localPlayer = PhotonNetwork.LocalPlayer;
        usernameSprites[0].GetComponentInChildren<UILabel>().text = localPlayer.NickName;

        if (localPlayer.IsMasterClient)
        {
            startGameButton.GetComponentInChildren<UILabel>().text = "开始游戏";
            usernameSprites[0].transform.Find("Name Label").GetComponent<UILabel>().color = Color.red;
        }
        else
        {
            if ((bool) PhotonNetwork.LocalPlayer.GetProperties("isReady", false))
            {
                usernameSprites[0].transform.Find("Name Label").GetComponent<UILabel>().color = Color.yellow;
                startGameButton.GetComponentInChildren<UILabel>().text = "取消准备";
            }
            else
            {
                usernameSprites[0].transform.Find("Name Label").GetComponent<UILabel>().color = Color.white;
                startGameButton.GetComponentInChildren<UILabel>().text = "准备";
            }
            
        }

        FindObjectOfType<ChoosePlane>().SetPlayerInt(localPlayer);

        FindObjectOfType<ShowPlane>().DestroyAll();
        for (int i = 1; i < 6; i++)
            usernameSprites[i].gameObject.SetActive(false);

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            Player player = PhotonNetwork.PlayerListOthers[i];

            usernameSprites[i + 1].gameObject.SetActive(true);
            usernameSprites[i + 1].GetComponentInChildren<UILabel>().text = player.NickName;

            if (localPlayer.IsMasterClient)
            {
                usernameSprites[i + 1].transform.Find("Master Label").gameObject.SetActive(true);
                usernameSprites[i + 1].transform.Find("Kick Label").gameObject.SetActive(true);
            }
            else
            {
                usernameSprites[i + 1].transform.Find("Master Label").gameObject.SetActive(false);
                usernameSprites[i + 1].transform.Find("Kick Label").gameObject.SetActive(false);
            }

            FindObjectOfType<ShowPlane>().Show(i);

            if (player.IsMasterClient)
                usernameSprites[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.red;
            else
            {
                if ((bool) player.GetProperties("isReady", false))
                    usernameSprites[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.yellow;
                else
                    usernameSprites[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.white;
            }
        }
    }

    public void OpenToggleValueChanged()
    {
        PhotonNetwork.CurrentRoom.IsVisible = openToggle.value;
    }

    public void KickLabelOnClick(UILabel name)
    {
        foreach (Player playersOthers in PhotonNetwork.PlayerListOthers)
        {
            if (playersOthers.NickName == name.text)
            {
                photonView.RPC("KickPlayer", playersOthers);
            }
        }
    }

    public void MasterLabelOnClick(UILabel name)
    {
        foreach (Player playerOthers in PhotonNetwork.PlayerListOthers)
        {
            if (playerOthers.NickName == name.text)
            {
                PhotonNetwork.CurrentRoom.SetMasterClient(playerOthers);
            }
        }
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
            bool isAllReady =
                PhotonNetwork.PlayerListOthers.All(playerOthers => (bool) playerOthers.GetProperties("isReady", false));
            if (isAllReady)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;

                PhotonNetwork.LoadLevel(FindObjectOfType<ChooseMap>().Index + 1);
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
                usernameSprites[0].GetComponentInChildren<UILabel>().color = Color.yellow;
                PhotonNetwork.LocalPlayer.SetProperties("isReady", true);
            }
            else
            {
                startGameButton.GetComponentInChildren<UILabel>().text = "准备";
                usernameSprites[0].GetComponentInChildren<UILabel>().color = Color.white;
                PhotonNetwork.LocalPlayer.SetProperties("isReady", false);
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

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        
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
            if (!Equals(target, PhotonNetwork.PlayerListOthers[i])) continue;

            if (target.IsMasterClient)
                usernameSprites[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.red;
            else
            {
                if ((bool) target.GetProperties("isReady", false))
                    usernameSprites[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.yellow;
                else
                    usernameSprites[i + 1].transform.Find("Name Label").GetComponent<UILabel>().color = Color.white;
            }
        }
    }
}
