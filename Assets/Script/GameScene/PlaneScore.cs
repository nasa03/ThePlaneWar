using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlaneScore : MonoBehaviour
{
    [System.Serializable]
    class Score
    {
        public Text scoreNameText;
        public Text scoreText;
        public int kill;
        public int death;
    }

    [SerializeField] Image[] panel = new Image[2];
    [SerializeField] Score[] scores = new Score[6];

    public void Show()
    {
        panel[0].enabled = true;
        panel[1].enabled = true;

        int planeCount = PhotonNetwork.PlayerList.Length;
        for(int i = 0; i < 6; i++)
        {
            if (i < planeCount)
            {
                scores[i].scoreNameText.enabled = true;
                scores[i].scoreText.enabled = true;

                scores[i].scoreNameText.text = PhotonNetwork.PlayerList[i].NickName;
                if (PhotonNetwork.PlayerList[i].IsLocal)
                    scores[i].scoreNameText.color = Color.red;

                scores[i].kill = (int)CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "kill");
                scores[i].death = (int)CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "death");
                scores[i].scoreText.text = string.Format("{0}/{1}", scores[i].kill, scores[i].death);
            }
            else
            {
                scores[i].scoreNameText.enabled = false;
                scores[i].scoreText.enabled = false;
            }
        }
    }

    public void Hide()
    {
        panel[0].enabled = false;
        panel[1].enabled = false;
        for(int i = 0; i < 6; i++)
        {
            scores[i].scoreNameText.enabled = false;
            scores[i].scoreText.enabled = false;
        }
    }
}
