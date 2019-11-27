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
    [SerializeField] GameObject GameOverUI;
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

        ClientStateHandler();
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void ClientStateHandler()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            if (Global.returnState == Global.ReturnState.exitGame)
            {
                if (!Global.isOffline)
                    FindObjectOfType<PhotonRoom>().LeftRoomButtonOnClick();
                else
                    OnJoinedRoom();

                FindObjectOfType<GameOver>().ResetInformation();
            }
            else GameOver();
        }

        if (Global.returnState == Global.ReturnState.disconnected)
            StartCoroutine(FindObjectOfType<MessageShow>().Show("已断开连接！"));

        Global.returnState = Global.ReturnState.normal;
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
    }

    public void GameOver()
    {
        LoginUI.SetActive(false);
        LobbyUI.SetActive(false);
        RoomUI.SetActive(false);
        GameOverUI.SetActive(true);

        FindObjectOfType<GameOver>().Show();
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

        ConnectButton.isEnabled = true;
        FindObjectOfType<ChoosePlane>().Show(false);

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
            TypedLobby typedLobby = new TypedLobby("MyLobby", LobbyType.SqlLobby);
            PhotonNetwork.JoinLobby(typedLobby);
        }
        else
        {
            PhotonNetwork.CreateRoom("离线模式");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        LoginUI.SetActive(false);
        LobbyUI.SetActive(false);
        RoomUI.SetActive(true);
        GameOverUI.SetActive(false);

        FindObjectOfType<ChoosePlane>().Show(true);
        FindObjectOfType<PhotonRoom>().EnterOrRefreshRoom();
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
