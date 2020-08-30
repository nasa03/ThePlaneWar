using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonGameAI : MonoBehaviourPun
{
    [SerializeField] GameObject[] AI_Plane_Prefabs;
    [SerializeField] Transform[] randomPositions;
    [HideInInspector] public ArrayList AI_Plane_List = new ArrayList();

    public Transform[] RandomPositions => randomPositions;

    public void InitializeAI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            object[] AI_Plane_Index = (object[]) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, "ai_List");

            if (AI_Plane_Index != null)
            {
                for (int i = 0; i < AI_Plane_Index.Length; i++)
                {
                    GameObject AI_Plane = PhotonNetwork.InstantiateRoomObject(
                        AI_Plane_Prefabs[(int) AI_Plane_Index[i]].name,
                        FindObjectOfType<PhotonGame>().GroundRunwayPosition[PhotonNetwork.CurrentRoom.Players.Count + i]
                            .position +
                        new Vector3(0, 15, 0), Quaternion.identity);

                    photonView.RPC("HandleAIPlaneProperty", RpcTarget.All, AI_Plane.name, i);
                }
            }
        }
    }

    [PunRPC]
    public void HandleAIPlaneProperty(string name, int index)
    {
        GameObject AI_Plane = GameObject.Find(name);

        AI_Plane.GetComponent<AIAttack>().Index = PhotonNetwork.CurrentRoom.Players.Count + index;
        AI_Plane.GetComponent<AIProperty>().Initialize(string.Format("机器人{0}", index + 1));
        AI_Plane_List.Add(AI_Plane);
    }

    [PunRPC]
    public void DeadAI(string targetName)
    {
        Transform target = GameObject.Find(targetName).transform;

        GameObject explosion = PhotonNetwork.Instantiate(FindObjectOfType<PhotonGame>().ExplosionParticleSystem.name,
            target.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        target.GetComponent<AudioPlayer>().PlayAudio(3);

        target.GetComponent<AIAttack>().RebornStart();

        if (PhotonNetwork.IsMasterClient)
        {
            AIProperty aiProperty = target.GetComponent<AIProperty>();
            aiProperty.Death++;
            aiProperty.HP = 100;
        }
    }
}
