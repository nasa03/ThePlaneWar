using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChoosePlane : MonoBehaviour {
    [SerializeField] GameObject[] planePrefabs;
    GameObject totalPlaneObject;

    public GameObject[] PlanePrefabs => planePrefabs;

    public void Show(bool isShow)
    {
        if (isShow)
        {
            totalPlaneObject = Instantiate(planePrefabs[Global.totalPlaneInt], Global.planePositions[0], Quaternion.Euler(0, 180, 0));
            totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);

            CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "totalPlaneInt", Global.totalPlaneInt);
        }
        else
        {
            Destroy(totalPlaneObject);
        }
    }

    public void NextPlane()
    {
        Global.totalPlaneInt++;
        if (Global.totalPlaneInt > planePrefabs.Length - 1)
            Global.totalPlaneInt = 0;

        ChangePlane();
    }

    public void LastPlane()
    {
        Global.totalPlaneInt--;
        if (Global.totalPlaneInt < 0)
            Global.totalPlaneInt = planePrefabs.Length - 1;

        ChangePlane();
    }

    void ChangePlane()
    {
        Destroy(totalPlaneObject);
        totalPlaneObject = Instantiate(planePrefabs[Global.totalPlaneInt], Global.planePositions[0], Quaternion.Euler(0, 180, 0));
        totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);

        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "totalPlaneInt", Global.totalPlaneInt);
    }

    public void SetPlayerInt(Player localPlayer)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (localPlayer == PhotonNetwork.PlayerList[i])
            {
                Global.totalPlayerInt = i;
            }
        }
    }
}
