using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] UIGrid roomGrid;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] UIScrollView scrollView;
    public Join join;
    public Create create;

    [System.Serializable]
    public class Join
    {
        public GameObject joinWidget;
        public UILabel nameLabel;
        public UILabel playersLabel;
        public UILabel openLabel;
        [HideInInspector] public RoomInfo totalRoonInfo;
    }
    [System.Serializable]
    public class Create
    {
        public GameObject CreateWidget;
        public UIInput nameInput;
        public MaxPlayersSlider maxPlayersSlider;
        public UIToggle openToggle;
    }

    public void JoinButtonOnClick()
    {
        PhotonNetwork.JoinRoom(join.totalRoonInfo.Name);
    }
    public void CreateBottonOnclick()
    {
        join.joinWidget.SetActive(false);
        create.CreateWidget.SetActive(true);
    }
    public void CreateEnterButtonOnclick()
    {
        string roomName = create.nameInput.value;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)create.maxPlayersSlider.MaxPlayers;
        options.IsOpen = create.openToggle.value;

        PhotonNetwork.CreateRoom(roomName, options, PhotonNetwork.CurrentLobby);
    }
    public void RefreshButtonOnClick()
    {
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        roomGrid.transform.DestroyChildren();

        foreach (RoomInfo roominfo in roomList)
        {
            GameObject room = Instantiate(roomPrefab, roomGrid.transform);
            room.transform.Find("RoomID Label").GetComponent<UILabel>().text = string.Format("ID：{0}", roominfo.masterClientId);
            room.transform.Find("RoomName Label").GetComponent<UILabel>().text = string.Format("Name：{0}", roominfo.Name);
            room.GetComponent<UIDragScrollView>().scrollView = scrollView;
            room.GetComponent<ChooseRoom>().roomInfo = roominfo;
        }

        roomGrid.Reposition();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("Create Room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        StartCoroutine(FindObjectOfType<MessageShow>().Show(message));
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        StartCoroutine(FindObjectOfType<MessageShow>().Show(message));
    }
}
