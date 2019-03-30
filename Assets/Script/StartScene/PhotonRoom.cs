using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] UISprite[] usernameSprides;
    [SerializeField] UIButton startGameButton;

    public void Refresh()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        usernameSprides[0].GetComponentInChildren<UILabel>().text = localPlayer.NickName;

        if (localPlayer.IsMasterClient)
            startGameButton.isEnabled = true;
        else
            startGameButton.isEnabled = false;

        for (int i = 1; i <= PhotonNetwork.PlayerListOthers.Length; i++)
        {
            Player player = PhotonNetwork.PlayerListOthers[i - 1];
            usernameSprides[i].GetComponentInChildren<UILabel>().text = player.NickName;

            if (player.IsMasterClient)
                usernameSprides[i].GetComponentInChildren<UILabel>().effectColor = Color.red;
        }

        for (int i = 0; i < 6; i++)
        {
            if (i > PhotonNetwork.PlayerListOthers.Length)
                usernameSprides[i].gameObject.SetActive(false);
            else
                usernameSprides[i].gameObject.SetActive(true);
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
