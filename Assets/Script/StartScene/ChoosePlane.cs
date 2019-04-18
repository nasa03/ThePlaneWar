//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ChoosePlane : MonoBehaviour {
    public GameObject[] planePrefabs;
    GameObject totalPlaneObject;
    int totalPlaneInt = 0;
    int totalPlayerInt = 0;
    public int TotalPlaneInt { get { return totalPlaneInt; } }
    public int TotalPlayerInt { get { return totalPlayerInt; } }
    Hashtable keyValuePairs = new Hashtable();

    public void Show(bool isShow)
    {
        if (isShow)
        {
            totalPlaneObject = Instantiate(planePrefabs[totalPlaneInt], Global.planePositions[0], Quaternion.Euler(0, 180, 0));
            totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);

            if (keyValuePairs.ContainsKey("totalPlaneInt"))
                keyValuePairs["totalPlaneInt"] = totalPlaneInt;
            else
                keyValuePairs.Add("totalPlaneInt", totalPlaneInt);

            PhotonNetwork.LocalPlayer.SetCustomProperties(keyValuePairs);
        }
        else
        {
            Destroy(totalPlaneObject);
        }
    }

    public void NextPlane()
    {
        totalPlaneInt++;
        if (totalPlaneInt > planePrefabs.Length - 1)
            totalPlaneInt = 0;

        ChangePlane();
    }

    public void LastPlane()
    {
        totalPlaneInt--;
        if (totalPlaneInt < 0)
            totalPlaneInt = planePrefabs.Length - 1;

        ChangePlane();
    }

    void ChangePlane()
    {
        Destroy(totalPlaneObject);
        totalPlaneObject = Instantiate(planePrefabs[totalPlaneInt], Global.planePositions[0], Quaternion.Euler(0, 180, 0));
        totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);

        if (keyValuePairs.ContainsKey("totalPlaneInt"))
            keyValuePairs["totalPlaneInt"] = totalPlaneInt;
        else
            keyValuePairs.Add("totalPlaneInt", totalPlaneInt);

        PhotonNetwork.LocalPlayer.SetCustomProperties(keyValuePairs);
    }

    public void SetPlayerInt(Player localPlayer)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (localPlayer == PhotonNetwork.PlayerList[i])
            {
                totalPlayerInt = i;
            }
        }
    }
}
