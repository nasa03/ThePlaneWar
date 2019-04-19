using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks {
    [SerializeField] UIGrid roomGrid;
    [SerializeField] UILabel emptyRoomLabel;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] UIScrollView scrollView;
    [SerializeField] GameObject CreateWidget;
    [SerializeField] UIInput nameInput;
    [SerializeField] MaxPlayersSlider maxPlayersSlider;
    [SerializeField] UIToggle openToggle;

    public void CreateButtonOnclick()
    {
        string roomName = nameInput.value;

        if (string.IsNullOrEmpty(roomName))
        {
            StartCoroutine(FindObjectOfType<MessageShow>().Show("房间名为空!"));
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayersSlider.MaxPlayers;
        options.IsOpen = openToggle.value;

        PhotonNetwork.CreateRoom(roomName, options, PhotonNetwork.CurrentLobby);
    }
    public void RefreshButtonOnClick()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
        }
    }
    public void DisconnectButtonOnClick()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        if (roomList.Count == 0)
            emptyRoomLabel.gameObject.SetActive(true);
        else
            emptyRoomLabel.gameObject.SetActive(false);

        roomGrid.transform.DestroyChildren();

        foreach (RoomInfo roomInfo in roomList)
        {
            GameObject room = Instantiate(roomPrefab);

            if (!roomInfo.IsOpen)
                continue;

            room.transform.parent = roomGrid.transform;
            room.transform.localPosition = Vector3.zero;
            room.transform.localScale = Vector3.one;
            room.transform.Find("Room Name Label").GetComponent<UILabel>().text = string.Format("Name：{0}", roomInfo.Name);
            room.transform.Find("Room Players Label").GetComponent<UILabel>().text= string.Format("人数：{0}/{1}", roomInfo.PlayerCount, roomInfo.MaxPlayers);
            room.GetComponent<UIDragScrollView>().scrollView = scrollView;
            room.GetComponent<ChooseRoom>().roomInfo = roomInfo;
        }

        roomGrid.Reposition();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        StartCoroutine(FindObjectOfType<MessageShow>().Show("加入房间失败!"));
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        StartCoroutine(FindObjectOfType<MessageShow>().Show("创建房间失败！"));
    }
}
