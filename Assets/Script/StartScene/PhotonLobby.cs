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
    Dictionary<string, RoomInfo> roomDictionary = new Dictionary<string, RoomInfo>();

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
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
        }
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

        foreach(RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
                roomDictionary.Remove(roomInfo.Name);
            if (roomDictionary.ContainsKey(roomInfo.Name))
                roomDictionary.Add(roomInfo.Name, roomInfo);
            else
                roomDictionary[roomInfo.Name] = roomInfo;
        }

        roomGrid.transform.DestroyChildren();

        foreach (RoomInfo roominfo in roomDictionary.Values)
        {
            GameObject room = Instantiate(roomPrefab);
            room.transform.parent = roomGrid.transform;
            room.transform.localScale = Vector3.one;
            room.transform.Find("Room ID Label").GetComponent<UILabel>().text = string.Format("ID：{0}", roominfo.masterClientId);
            room.transform.Find("Room Name Label").GetComponent<UILabel>().text = string.Format("Name：{0}", roominfo.Name);
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
