using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShowPlane : MonoBehaviourPunCallbacks {
    GameObject[] otherPlaneObject = new GameObject[5];

    public void Show(int playerInt)
    {
        Player player = PhotonNetwork.PlayerListOthers[playerInt];
        int planeInt = (int)CustomProperties.GetProperties(player, "totalPlaneInt", 0);

        otherPlaneObject[playerInt] = Instantiate(FindObjectOfType<ChoosePlane>().planePrefabs[planeInt],
            Global.planePositions[playerInt + 1], Quaternion.Euler(0, 180, 0));
        otherPlaneObject[playerInt].transform.localScale = new Vector3(5, 5, 5);
    }

    public void ShowAI(int playerInt,int planeInt)
    {
        otherPlaneObject[playerInt] = Instantiate(FindObjectOfType<ChoosePlane>().planePrefabs[planeInt],
            Global.planePositions[playerInt + 1], Quaternion.Euler(0, 180, 0));
        otherPlaneObject[playerInt].transform.localScale = new Vector3(5, 5, 5);
    }

    public void DestroyAI(int playerInt)
    {
        Destroy(otherPlaneObject[playerInt]);
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
                int planeInt = (int)CustomProperties.GetProperties(target, "totalPlaneInt", 0);
                Destroy(otherPlaneObject[i]);
                otherPlaneObject[i] = Instantiate(FindObjectOfType<ChoosePlane>().planePrefabs[planeInt],
                    Global.planePositions[i + 1], Quaternion.Euler(0, 180, 0));
                otherPlaneObject[i].transform.localScale = new Vector3(5, 5, 5);
            }
        }
    }
}
