using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks {
    [SerializeField] UISprite[] usernameSprides;
    [SerializeField] UIButton startGameButton;

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

        for (int i = 1; i <= PhotonNetwork.PlayerListOthers.Length; i++)
        {
            Player player = PhotonNetwork.PlayerListOthers[i - 1];
            usernameSprides[i].GetComponentInChildren<UILabel>().text = player.NickName;

            if (player.IsMasterClient)
                usernameSprides[i].GetComponentInChildren<UILabel>().color = Color.red;
            else
                usernameSprides[i].GetComponentInChildren<UILabel>().color = Color.white;
        }

        for (int i = 0; i < 6; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length)
            {
                usernameSprides[i].gameObject.SetActive(true);
                FindObjectOfType<ShowPlane>().Show(i, true);
            }
            else
            {
                usernameSprides[i].gameObject.SetActive(false);
                FindObjectOfType<ShowPlane>().Show(i, false);
            }
        }
    }

    public void LeftRoomButtonOnClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void StartGameButtonOnClick()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Refresh();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Refresh();
    }
}
