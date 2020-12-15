using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class PhotonGameAI : MonoBehaviourPun
{
    [SerializeField] GameObject[] aiPlanePrefabs;
    [SerializeField] Transform[] randomPositions;
    [HideInInspector] public ArrayList aiPlaneList = new ArrayList();

    public Transform[] RandomPositions => randomPositions;

    public IEnumerator InitializeAI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            while (true)
            {
                if (!PhotonNetwork.PlayerList.All(player => (bool) player.GetProperties("isLoadScene", false)))
                {
                    yield return new WaitForSeconds(1.0f);
                    continue;
                }

                break;
            }

            object[] aiPlaneIndex = (object[]) PhotonNetwork.CurrentRoom.GetProperties("ai_List");

            if (aiPlaneIndex != null)
            {
                for (int i = 0; i < aiPlaneIndex.Length; i++)
                {
                    GameObject aiPlane = PhotonNetwork.InstantiateRoomObject(
                        aiPlanePrefabs[(int) aiPlaneIndex[i]].name,
                        FindObjectOfType<PhotonGame>().GroundRunwayPosition[PhotonNetwork.CurrentRoom.Players.Count + i]
                            .position +
                        new Vector3(0, 15, 0), Quaternion.identity);

                    photonView.RPC("HandleAIPlaneProperty", RpcTarget.All, aiPlane.name, i);
                }
            }
        }
    }

    [PunRPC]
    public void HandleAIPlaneProperty(string name, int index)
    {
        GameObject aiPlane = GameObject.Find(name);

        aiPlane.GetComponent<AIAttack>().Index = PhotonNetwork.CurrentRoom.Players.Count + index;
        aiPlane.GetComponent<AIProperty>().Initialize($"机器人{index + 1}");
        aiPlaneList.Add(aiPlane);
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

    public void HandleAIPlaneScores()
    {
        foreach (GameObject aiPlane in aiPlaneList)
        {
            Global.aiPlaneScores.Add(aiPlane.GetComponent<AIProperty>().aiPlaneScores);
        }
    }
}
