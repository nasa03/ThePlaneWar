using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(playerPrefab, startPositions[playerControllerId].position, Quaternion.identity);
        //player.GetComponent<MeshFilter>().mesh = playerMesh[playerControllerId];
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
