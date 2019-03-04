using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InputName : MonoBehaviour
{
    [SerializeField] UIInput nameInput;
    string playerName = string.Empty;
    const string playerNamePrefKey = "playerName";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
            playerName = PlayerPrefs.GetString(playerNamePrefKey);
    }

    public void Input_OnValueChanged()
    {
        playerName = nameInput.value;
    }

    public void Button_OnClicked()
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("姓名为空！");
            return;
        }

        PlayerPrefs.SetString(playerNamePrefKey, playerName);

        PhotonNetwork.NickName = playerName;
    }
}
