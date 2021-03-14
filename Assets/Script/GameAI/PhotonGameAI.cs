using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonGameAI : MonoBehaviourPun
{
    [SerializeField] private GameObject[] aiPlanePrefabs;
    [SerializeField] private Transform[] randomPositions;

    public Transform[] RandomPositions => randomPositions;

    public List<GameObject> AiPlaneList { get; } = new List<GameObject>();

    public IEnumerator InitializeAI()
    {
        object[] aiPlaneIndex = (object[]) PhotonNetwork.CurrentRoom.GetProperties("ai_List");

        if (aiPlaneIndex == null) yield break;

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < aiPlaneIndex.Length; i++)
            {
                GameObject aiPlane = PhotonNetwork.InstantiateRoomObject(
                    aiPlanePrefabs[(int) aiPlaneIndex[i]].name,
                    FindObjectOfType<PhotonGame>().GroundRunwayPosition[PhotonNetwork.CurrentRoom.Players.Count + i]
                        .position + new Vector3(0, 15, 0), Quaternion.identity);

                photonView.RPC("HandleAIPlaneProperty", RpcTarget.All, aiPlane.name, i);
            }
        }

        yield return new WaitUntil(() => AiPlaneList.Count == aiPlaneIndex.Length);
    }

    [PunRPC]
    public void HandleAIPlaneProperty(string name, int index)
    {
        GameObject aiPlane = GameObject.Find(name);

        aiPlane.GetComponent<AIAttack>().Index = PhotonNetwork.CurrentRoom.Players.Count + index;
        aiPlane.GetComponent<AIProperty>().Initialize($"机器人{index + 1}");
        AiPlaneList.Add(aiPlane);
    }

    [PunRPC]
    public void DeadAI(string targetName)
    {
        Transform target = GameObject.Find(targetName).transform;

        GameObject explosion = PhotonNetwork.Instantiate(FindObjectOfType<PhotonGame>().ExplosionParticleSystem.name,
            target.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        target.GetComponent<AudioPlayer>().PlayAudio(3);

        if (PhotonNetwork.IsMasterClient)
        {
            AIProperty aiProperty = target.GetComponent<AIProperty>();
            aiProperty.Death++;
            aiProperty.HP = 100;
            aiProperty.isDead = true;
        }

        StartCoroutine(target.GetComponent<AIAttack>().RebornStart());
    }

    public void HandleAIPlaneScores()
    {
        AiPlaneList.ForEach(aiPlane => Global.aiPlaneScores.Add(aiPlane.GetComponent<AIProperty>().aiPlaneScores));
    }
}