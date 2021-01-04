using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject lobbyUI;
    [SerializeField] private GameObject roomUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private UIInput nameInput;
    [SerializeField] private UIButton connectButton;
    [SerializeField] private UILabel idLabel;
    private string _playerName = string.Empty;

    // Start is called before the first frame update
    private void Start()
    {
        if (!File.Exists(Global.Path))
            return;

        FileStream stream = new FileStream(Global.Path, FileMode.Open);
        StreamReader reader = new StreamReader(stream);
        _playerName = reader.ReadToEnd();
        nameInput.value = _playerName;
        reader.Close();
        stream.Close();

        ClientStateHandler();
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void ClientStateHandler()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            if (Global.returnState == Global.ReturnState.ExitGame)
            {
                if (!Global.isOffline)
                    FindObjectOfType<PhotonRoom>().LeftRoomButtonOnClick();
                else
                    OnJoinedRoom();

                FindObjectOfType<GameOver>().ResetInformation();
            }
            else GameOver();
        }

        if (Global.returnState == Global.ReturnState.Disconnected)
            StartCoroutine(FindObjectOfType<MessageShow>().Show("已断开连接！"));
    }

    public void InputValueChanged()
    {
        _playerName = nameInput.value;
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(_playerName))
        {
            StartCoroutine(FindObjectOfType<MessageShow>().Show("ID为空！"));
            return;
        }

        FileStream stream = new FileStream(Global.Path, FileMode.Create);
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(_playerName);
        writer.Flush();
        writer.Close();
        stream.Close();

        PhotonNetwork.NickName = _playerName;

        connectButton.isEnabled = false;
        if (!Global.isOffline)
            PhotonNetwork.ConnectUsingSettings();
        else
            PhotonNetwork.OfflineMode = true;
    }

    public void OnSettingButtonClick()
    {
        loginUI.SetActive(false);
        lobbyUI.SetActive(false);
        roomUI.SetActive(false);
        settingUI.SetActive(true);
        gameOverUI.SetActive(false);
        
        FindObjectOfType<JsonController>().ShowSettingUI();
    }

    public void OnSettingReturnButtonClick()
    {
        loginUI.SetActive(true);
        lobbyUI.SetActive(false);
        roomUI.SetActive(false);
        settingUI.SetActive(false);
        gameOverUI.SetActive(false);
        
        FindObjectOfType<JsonController>().Save();
    }

    public void OnExitGameButtonClick()
    {
        Application.Quit();
    }

    private void GameOver()
    {
        loginUI.SetActive(false);
        lobbyUI.SetActive(false);
        roomUI.SetActive(false);
        gameOverUI.SetActive(true);
        
        FindObjectOfType<GameOver>().Show();
    }

    public override void OnConnected()
    {
        base.OnConnected();

        loginUI.SetActive(false);
        lobbyUI.SetActive(true);
        roomUI.SetActive(false);

        idLabel.text = $"用户名：{PhotonNetwork.NickName}";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        connectButton.isEnabled = true;

        StartCoroutine(FindObjectOfType<MessageShow>().Show("已断开连接！"));

        loginUI.SetActive(true);
        lobbyUI.SetActive(false);
        roomUI.SetActive(false);

        FindObjectOfType<ChoosePlane>().Show(false);
        FindObjectOfType<ShowPlane>().DestroyAll();
        FindObjectOfType<PhotonAI>().LeftRoomOfAI();
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

        loginUI.SetActive(false);
        lobbyUI.SetActive(false);
        roomUI.SetActive(true);
        gameOverUI.SetActive(false);
        
        Global.returnState = Global.ReturnState.Normal;
        PhotonNetwork.LocalPlayer.SetProperties("isLoadScene", false);

        FindObjectOfType<ChoosePlane>().Show(true);
        FindObjectOfType<PhotonRoom>().EnterOrRefreshRoom();
        FindObjectOfType<PhotonAI>().EnterOrRefreshRoomOfAI();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        loginUI.SetActive(false);
        lobbyUI.SetActive(true);
        roomUI.SetActive(false);

        FindObjectOfType<ChoosePlane>().Show(false);
        FindObjectOfType<ShowPlane>().DestroyAll();
        FindObjectOfType<PhotonAI>().LeftRoomOfAI();
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
