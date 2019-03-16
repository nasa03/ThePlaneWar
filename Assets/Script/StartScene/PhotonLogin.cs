using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject LoginUI;
    [SerializeField] GameObject LobbyUI;
    [SerializeField] UIInput nameInput;
    [SerializeField] UIButton ConnectButton;
    [SerializeField] UILabel IDLabel;
    string playerName = string.Empty;
    const string playerNamePrefKey = "playerName";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
            playerName = PlayerPrefs.GetString(playerNamePrefKey);
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

        PlayerPrefs.SetString(playerNamePrefKey, playerName);

        PhotonNetwork.NickName = playerName;

        ConnectButton.isEnabled = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        base.OnConnected();

        LoginUI.SetActive(false);
        LobbyUI.SetActive(true);

        IDLabel.text = string.Format("用户名：{0}", PlayerPrefs.GetString(playerNamePrefKey));
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        ConnectButton.isEnabled = true;

        LoginUI.SetActive(true);
        LobbyUI.SetActive(false);
    }
}
