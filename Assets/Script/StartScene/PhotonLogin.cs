using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
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
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        DontDestroyOnLoad(this);
    }

    public void InputValueChanged()
    {
        playerName = nameInput.value;
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("姓名为空！");
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
        PhotonNetwork.ConnectUsingSettings();
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

        LoginUI.SetActive(true);
        LobbyUI.SetActive(false);
        RoomUI.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Name = "MyLobby";
        typedLobby.Type = LobbyType.SqlLobby;
        PhotonNetwork.JoinLobby(typedLobby);
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        LoginUI.SetActive(false);
        LobbyUI.SetActive(false);
        RoomUI.SetActive(true);

        FindObjectOfType<ChoosePlane>().Show(true);
        FindObjectOfType<PhotonRoom>().Refresh();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        LoginUI.SetActive(false);
        LobbyUI.SetActive(true);
        RoomUI.SetActive(false);

        FindObjectOfType<ChoosePlane>().Show(false);
    }
}
