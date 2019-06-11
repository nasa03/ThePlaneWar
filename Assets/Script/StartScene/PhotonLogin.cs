using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonLogin : MonoBehaviourPunCallbacks {
    [SerializeField] GameObject LoginUI;
    [SerializeField] GameObject LobbyUI;
    [SerializeField] GameObject RoomUI;
    [SerializeField] UIInput nameInput;
    [SerializeField] UIButton ConnectButton;
    [SerializeField] UILabel IDLabel;
    string playerName = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        if (!File.Exists(Global.path))
            return;

        FileStream stream = new FileStream(Global.path, FileMode.Open);
        StreamReader reader = new StreamReader(stream);
        playerName = reader.ReadToEnd();
        nameInput.value = playerName;
        reader.Close();
        stream.Close();

        if (Global.inGame)
            PhotonNetwork.Disconnect();
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
    }

    public void InputValueChanged()
    {
        playerName = nameInput.value;
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(playerName))
        {
            StartCoroutine(FindObjectOfType<MessageShow>().Show("ID为空！"));
            return;
        }

        FileStream stream = new FileStream(Global.path, FileMode.Create);
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(playerName);
        writer.Flush();
        writer.Close();
        stream.Close();

        PhotonNetwork.NickName = playerName;

        ConnectButton.isEnabled = false;
        if (!Global.isOffline)
            PhotonNetwork.ConnectUsingSettings();
        else
            PhotonNetwork.OfflineMode = true;

        Global.inGame = false;
    }

    public override void OnConnected()
    {
        base.OnConnected();

        LoginUI.SetActive(false);
        LobbyUI.SetActive(true);
        RoomUI.SetActive(false);

        IDLabel.text = string.Format("用户名：{0}", PhotonNetwork.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        if (Global.inGame)
        {
            Connect();
            return;
        }

        ConnectButton.isEnabled = true;

        StartCoroutine(FindObjectOfType<MessageShow>().Show("已断开连接！"));

        LoginUI.SetActive(true);
        LobbyUI.SetActive(false);
        RoomUI.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if (!PhotonNetwork.OfflineMode)
        {
            TypedLobby typedLobby = new TypedLobby();
            typedLobby.Name = "MyLobby";
            typedLobby.Type = LobbyType.SqlLobby;
            PhotonNetwork.JoinLobby(typedLobby);
        }
        else
        {
            PhotonNetwork.CreateRoom("离线模式房间");
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        LoginUI.SetActive(false);
        LobbyUI.SetActive(false);
        RoomUI.SetActive(true);

        FindObjectOfType<ChoosePlane>().Show(true);
        FindObjectOfType<PhotonRoom>().Refresh();
        FindObjectOfType<PhotonRoom>().EnterRoom();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        LoginUI.SetActive(false);
        LobbyUI.SetActive(true);
        RoomUI.SetActive(false);

        FindObjectOfType<ChoosePlane>().Show(false);
        FindObjectOfType<ShowPlane>().DestroyAll();
    }
}
