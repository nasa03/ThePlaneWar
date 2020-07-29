using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonScore : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    class Score
    {
        public Image scoreImage;
        public Text scoreText;
    }

    [SerializeField] Score title;
    [SerializeField] Score[] scores = new Score[6];

    public void Show()
    {
        title.scoreImage.enabled = true;
        title.scoreText.enabled = true;

        int planeCount = PhotonNetwork.PlayerList.Length;
        ArrayList aiPlaneList = FindObjectOfType<PhotonGameAI>().AI_Plane_List;
        for(int i = 0; i < 6; i++)
        {
            if (i < planeCount+aiPlaneList.Count)
            {
                if (i < planeCount)
                {
                    scores[i].scoreImage.enabled = true;
                    scores[i].scoreText.enabled = true;

                    string name = PhotonNetwork.PlayerList[i].NickName;
                    string kill = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "kill", 0).ToString();
                    string dead = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "death", 0).ToString();
                    scores[i].scoreText.text = string.Format("{0} {1}/{2}", name, kill, dead);
                    if (PhotonNetwork.PlayerList[i].IsLocal)
                        scores[i].scoreText.color = Color.red;
                }
                else
                {
                    scores[i].scoreImage.enabled = true;
                    scores[i].scoreText.enabled = true;

                    GameObject aiPlane = (GameObject) aiPlaneList[i - planeCount];
                    AIScore aiScore = aiPlane.GetComponent<AIScore>();
                    if (aiScore.Name == null)
                        aiScore.Initialize(string.Format("机器人{0}", i - planeCount + 1));
                    string name = aiScore.Name;
                    string kill = aiScore.Kill.ToString();
                    string dead = aiScore.Death.ToString();
                    scores[i].scoreText.text = string.Format("{0} {1}/{2}", name, kill, dead);
                    scores[i].scoreText.color = Color.green;
                }
                
            }
            else
            {
                scores[i].scoreImage.enabled = false;
                scores[i].scoreText.enabled = false;
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        Show();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Show();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Show();
    }
}
