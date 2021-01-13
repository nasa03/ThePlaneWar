using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks {
    [SerializeField] private UIGrid roomGrid;
    [SerializeField] private UILabel emptyRoomLabel;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private UIScrollView scrollView;
    [SerializeField] private UIInput nameInput;
    [SerializeField] private MaxPlayersSlider maxPlayersSlider;
    [SerializeField] private UIToggle openToggle;

    public void CreateButtonOnclick()
    {
        string roomName = nameInput.value;

        if (string.IsNullOrEmpty(roomName))
        {
            StartCoroutine(FindObjectOfType<MessageShow>().Show("请填写房间名！"));
            return;
        }

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = (byte) maxPlayersSlider.MaxPlayers, IsOpen = openToggle.value
        };

        PhotonNetwork.CreateRoom(roomName, options, PhotonNetwork.CurrentLobby);
    }
    
    public void JoinRandomButtonOnClick()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    
    public void DisconnectButtonOnClick()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        emptyRoomLabel.gameObject.SetActive(roomList.Count == 0);

        roomGrid.transform.DestroyChildren();

        roomList.ForEach(delegate(RoomInfo roomInfo)
        {
            GameObject room = Instantiate(roomPrefab, roomGrid.transform, true);

            if (!roomInfo.IsOpen)
                return;

            room.transform.localPosition = Vector3.zero;
            room.transform.localScale = Vector3.one;
            room.transform.Find("Room Name Label").GetComponent<UILabel>().text =
                $"房间名：{roomInfo.Name}";
            room.transform.Find("Room Players Label").GetComponent<UILabel>().text =
                $"人数：{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
            room.GetComponent<UIDragScrollView>().scrollView = scrollView;
            room.GetComponent<ChooseRoom>().roomInfo = roomInfo;
        });

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
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        StartCoroutine(FindObjectOfType<MessageShow>().Show("随机加入房间失败"));
    }
}
