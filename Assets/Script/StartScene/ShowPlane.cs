using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShowPlane : MonoBehaviourPunCallbacks {
    GameObject[] otherPlaneObject = new GameObject[5];

    public void Show(int PlayerInt)
    {
        Player player = PhotonNetwork.PlayerListOthers[PlayerInt];
        int planeInt = (int)CustomProperties.GetProperties(player, "totalPlaneInt");

        otherPlaneObject[PlayerInt] = Instantiate(FindObjectOfType<ChoosePlane>().planePrefabs[planeInt], Global.planePositions[PlayerInt + 1], Quaternion.Euler(0, 180, 0));
        otherPlaneObject[PlayerInt].transform.localScale = new Vector3(5, 5, 5);
    }

    public void DestroyAll()
    {
        for (int i = 0; i < otherPlaneObject.Length; i++)
            Destroy(otherPlaneObject[i]);
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (PhotonNetwork.PlayerListOthers[i] == target)
            {
                int planeInt = (int)CustomProperties.GetProperties(target, "totalPlaneInt");
                Destroy(otherPlaneObject[i]);
                otherPlaneObject[i] = Instantiate(FindObjectOfType<ChoosePlane>().planePrefabs[planeInt], Global.planePositions[i + 1], Quaternion.Euler(0, 180, 0));
                otherPlaneObject[i].transform.localScale = new Vector3(5, 5, 5);
            }
        }
    }
}
