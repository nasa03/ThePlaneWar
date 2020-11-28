using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChoosePlane : MonoBehaviour
{
    [SerializeField] private GameObject[] planePrefabs;
    private GameObject _totalPlaneObject;

    public GameObject[] PlanePrefabs => planePrefabs;

    public void Show(bool isShow)
    {
        if (isShow)
        {
            _totalPlaneObject = Instantiate(planePrefabs[Global.totalPlaneInt], Global.PlanePositions[0],
                Quaternion.Euler(0, 180, 0));
            _totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);

            PhotonNetwork.LocalPlayer.SetProperties("totalPlaneInt", Global.totalPlaneInt);
        }
        else
        {
            Destroy(_totalPlaneObject);
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

    private void ChangePlane()
    {
        Destroy(_totalPlaneObject);
        _totalPlaneObject = Instantiate(planePrefabs[Global.totalPlaneInt], Global.PlanePositions[0],
            Quaternion.Euler(0, 180, 0));
        _totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);

        PhotonNetwork.LocalPlayer.SetProperties("totalPlaneInt", Global.totalPlaneInt);
    }

    public void SetPlayerInt(Player localPlayer)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (Equals(localPlayer, PhotonNetwork.PlayerList[i]))
            {
                Global.totalPlayerInt = i;
            }
        }
    }
}