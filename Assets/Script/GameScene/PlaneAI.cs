using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlaneAI : MonoBehaviour
{
    [SerializeField] GameObject[] AI_Plane_Prefabs;
    private ArrayList AI_Plane_List;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int[] AI_Plane_Index = (int[]) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, "AI_List");
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            for (int i = 0; i < 6; i++)
            {
                GameObject AI_Plane = PhotonNetwork.Instantiate(AI_Plane_Prefabs[AI_Plane_Index[i]].name,
                    FindObjectOfType<PhotonGame>().GroundRunwayPosotion[playerCount + i].position, Quaternion.identity);
                AI_Plane_List.Add(AI_Plane_List);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
