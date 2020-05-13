using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlaneAI : MonoBehaviour
{
    [SerializeField] GameObject[] AI_Plane_Prefabs;
    private ArrayList AI_Plane_List = new ArrayList();
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            object[] AI_Plane_Index = (object[]) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, "AI_List");
            int playerCount = PhotonNetwork.CurrentRoom.Players.Count;
            for (int i = 0; i < AI_Plane_Index.Length; i++)
            {
                GameObject AI_Plane = PhotonNetwork.Instantiate(
                    AI_Plane_Prefabs[(int) AI_Plane_Index[i]].name,
                    FindObjectOfType<PhotonGame>().GroundRunwayPosotion[playerCount + i].position +
                    new Vector3(0, 15, 0), Quaternion.identity);
                AI_Plane_List.Add(AI_Plane);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
