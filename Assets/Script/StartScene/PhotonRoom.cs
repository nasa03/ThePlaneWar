using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] UIGrid playersGrid;
    [SerializeField] GameObject playerPrefab;

    public void Refresh()
    {
        playersGrid.transform.DestroyChildren();

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerObject = Instantiate(playerPrefab);
            playerObject.transform.parent = playersGrid.transform;
            playerObject.transform.position = Vector3.zero;
            playerObject.transform.localScale = Vector3.one;
            UILabel label = playerObject.transform.Find("Label").GetComponent<UILabel>();
            label.text = player.NickName;

            if (player.IsMasterClient) label.color = Color.red;
            else if (player.IsLocal) label.color = Color.green;
        }

        playersGrid.Reposition();
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
