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

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, null);
    }

    public void RefreshButtonOnClick()
    {
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
            ChooseRoom.Instance.roomInfo = roominfo;
            EventDelegate.Add(room.transform.Find("Control - Room Button").GetComponent<UIButton>().onClick, ChooseRoom.Instance.Show);
        }

        roomGrid.Reposition();
    }
}
