using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {
    [SerializeField] GameObject[] playerPrefabs;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(playerPrefabs[ChoosePlane.Instance.TotalPlaneInt], startPositions[playerControllerId].position, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
