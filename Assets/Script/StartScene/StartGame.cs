using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Net;
using System.Net.Sockets;

public class StartGame : MonoBehaviour {
    [SerializeField] NetworkManager networkManager;
    [SerializeField] UIPopupList networkList;
    [SerializeField] UIPopupList serverList;
    string ipAddress = "";
    List<MatchInfoSnapshot> serverMatchList = null;

    // Start is called before the first frame update
    void Start()
    {
        networkManager.StartMatchMaker();

        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
            }
        }
    }

    public void OnNetworkListValueChanged()
    {
        if (networkList.value == "作为客户端")
        {
            serverList.gameObject.SetActive(true);
            networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchesList);
        }
        else
            serverList.gameObject.SetActive(false);
    }

    public void OnStartButtonClick()
    {
        switch (networkList.GetComponentInChildren<UILabel>().text)
        {
            case "作为主机":
                networkManager.matchMaker.CreateMatch(ipAddress, 4, true, "", "", "", 0, 0, OnMatchCreateHost);
                break;
            case "作为客户端":
                int index = 0;
                for(int i=0; i<serverList.items.Count;i++)
                {
                    if(serverList.value==serverList.items[i])
                    {
                        index = i;
                        break;
                    }
                }
                networkManager.matchMaker.JoinMatch(serverMatchList[index].networkId, "", "", "", 0, 0, OnMatchJoined);
                break;
            case "作为服务器":
                networkManager.matchMaker.CreateMatch(ipAddress, 4, true, "", "", "", 0, 0, OnMatchCreateServer);
                break;
        }
    }

    private void OnMatchesList(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData)
    {
        if (!success)
        {
            Debug.LogWarning("List Matches Failed!");
            return;
        }

        serverList.items.Clear();

        foreach (MatchInfoSnapshot info in responseData)
        {
            serverList.AddItem(info.name);
        }

        serverMatchList = responseData;
    }

    private void OnMatchCreateHost(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (!success)
        {
            Debug.LogWarning("Create Match Failed!");
            return;
        }

        networkManager.StartHost(responseData);
    }

    private void OnMatchJoined(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (!success)
        {
            Debug.LogWarning("Join Match Failed!");
            return;
        }

        networkManager.StartClient(responseData);
    }


    private void OnMatchCreateServer(bool success, string extendedInfo, MatchInfo responseData)
    {
        if (!success)
        {
            Debug.LogWarning("Create Match Failed!");
            return;
        }
        
        networkManager.StartServer(responseData);
    }
}
