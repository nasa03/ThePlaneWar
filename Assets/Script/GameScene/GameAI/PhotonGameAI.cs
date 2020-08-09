using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonGameAI : MonoBehaviour
{
    [SerializeField] GameObject[] AI_Plane_Prefabs;
    [SerializeField] Transform[] randomPositions;
    [HideInInspector] public ArrayList AI_Plane_List = new ArrayList();

    public Transform[] RandomPositions => randomPositions;

    public void Initialize()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            object[] AI_Plane_Index = (object[]) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, "ai_List");
            int playerCount = PhotonNetwork.CurrentRoom.Players.Count;
            
            if (AI_Plane_Index != null)
            {
                for (int i = 0; i < AI_Plane_Index.Length; i++)
                {
                    GameObject AI_Plane = PhotonNetwork.InstantiateSceneObject(
                        AI_Plane_Prefabs[(int) AI_Plane_Index[i]].name,
                        FindObjectOfType<PhotonGame>().GroundRunwayPosition[playerCount + i].position +
                        new Vector3(0, 15, 0), Quaternion.identity);
                    AI_Plane.GetComponent<AIAttack>().Index = playerCount + i;
                    AI_Plane_List.Add(AI_Plane);
                }
            }
        }
    }
}
