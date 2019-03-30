//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ChoosePlane : MonoBehaviour {
    public GameObject[] planePrefabs;
    GameObject totalPlaneObject;
    int totalPlaneInt = 0;
    public int TotalPlaneInt { get { return totalPlaneInt; } }
    Hashtable keyValuePairs = new Hashtable();

    public void Show(bool isShow)
    {
        if (isShow)
        {
            totalPlaneObject = Instantiate(planePrefabs[totalPlaneInt], Global.planePositions[0], Quaternion.Euler(0, 180, 0));
            totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);
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
}
