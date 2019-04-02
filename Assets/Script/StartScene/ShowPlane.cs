//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ShowPlane : MonoBehaviourPunCallbacks {
    GameObject[] otherPlaneObject = new GameObject[5];

    public void Show(int PlayerInt)
    {
        if (PlayerInt == 0)
            return;

        otherPlaneObject[PlayerInt - 1] = Instantiate(FindObjectOfType<ChoosePlane>().planePrefabs[0], Global.planePositions[PlayerInt], Quaternion.Euler(0, 180, 0));
        otherPlaneObject[PlayerInt - 1].transform.localScale = new Vector3(5, 5, 5);
    }

    public void DestroyAll()
    {
        for (int i = 0; i < otherPlaneObject.Length; i++)
            Destroy(otherPlaneObject[i]);
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (PhotonNetwork.PlayerListOthers[i] == target)
            {
                int planeInt = (int)changedProps["totalPlaneInt"];
                Destroy(otherPlaneObject[i]);
                otherPlaneObject[i] = Instantiate(FindObjectOfType<ChoosePlane>().planePrefabs[planeInt], Global.planePositions[i + 1], Quaternion.Euler(0, 180, 0));
                otherPlaneObject[i].transform.localScale = new Vector3(5, 5, 5);
            }
        }
    }
}
