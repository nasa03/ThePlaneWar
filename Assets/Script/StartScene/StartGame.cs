using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartGame : MonoBehaviour {
    [SerializeField] NetworkManager networkManager;
    [SerializeField] UIPopupList popupList;

    public void OnClick()
    {
        switch (popupList.GetComponentInChildren<UILabel>().text)
        {
            case "作为主机":
                networkManager.StartHost();
                break;
            case "作为客户端":
                networkManager.StartClient();
                break;
            case "作为服务器":
                networkManager.StartServer();
                break;
        }
    }
}
