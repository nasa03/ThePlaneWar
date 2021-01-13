using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ShowPlane : MonoBehaviourPunCallbacks
{
    private readonly GameObject[] _otherPlaneObject = new GameObject[5];

    public void Show(int playerInt)
    {
        Player player = PhotonNetwork.PlayerListOthers[playerInt];
        int planeInt = (int) player.GetProperties("totalPlaneInt", 0);

        _otherPlaneObject[playerInt] = Instantiate(FindObjectOfType<ChoosePlane>().PlanePrefabs[planeInt],
            Global.PlanePositions[playerInt + 1], Quaternion.Euler(0, 180, 0));
        _otherPlaneObject[playerInt].transform.localScale = new Vector3(5, 5, 5);
    }

    public void ShowAI(int playerInt, int planeInt)
    {
        _otherPlaneObject[playerInt] = Instantiate(FindObjectOfType<ChoosePlane>().PlanePrefabs[planeInt],
            Global.PlanePositions[playerInt + 1], Quaternion.Euler(0, 180, 0));
        _otherPlaneObject[playerInt].transform.localScale = new Vector3(5, 5, 5);
    }

    public void DestroyAI(int playerInt)
    {
        Destroy(_otherPlaneObject[playerInt]);
    }

    public void DestroyAll()
    {
        _otherPlaneObject.ToList().ForEach(Destroy);
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        if (Global.returnState == Global.ReturnState.GameOver) return;
        
        for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
        {
            if (!Equals(PhotonNetwork.PlayerListOthers[i], target)) continue;
            
            int planeInt = (int) target.GetProperties("totalPlaneInt", 0);
            Destroy(_otherPlaneObject[i]);
            _otherPlaneObject[i] = Instantiate(FindObjectOfType<ChoosePlane>().PlanePrefabs[planeInt],
                Global.PlanePositions[i + 1], Quaternion.Euler(0, 180, 0));
            _otherPlaneObject[i].transform.localScale = new Vector3(5, 5, 5);
        }
    }
}
