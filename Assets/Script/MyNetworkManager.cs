using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager {
    [SerializeField] Mesh[] playerMesh;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        //设置player的一些参数
        player.GetComponent<MeshFilter>().mesh = playerMesh[playerControllerId];
        MeshRenderer renderer = player.GetComponent<MeshRenderer>();
        BoxCollider collider = player.GetComponent<BoxCollider>();
        collider.center = renderer.bounds.center;
        collider.size = renderer.bounds.size;

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
