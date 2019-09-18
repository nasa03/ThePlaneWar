using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityStandardAssets.CrossPlatformInput;

public class PlaneScore : MonoBehaviour
{
    [System.Serializable]
    class Score
    {
        public Text nameText;
        public Text killText;
        public Text deathText;
    }

    [SerializeField] Image panel;
    [SerializeField] Text[] title;
    [SerializeField] Score[] scores = new Score[6];

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Tab"))
        {
            Show();
        }

        if (CrossPlatformInputManager.GetButtonUp("Tab"))
        {
            Hide();
        }
    }

    public void Show()
    {
        panel.enabled = true;
        title[0].enabled = true;
        title[1].enabled = true;

        int planeCount = PhotonNetwork.PlayerList.Length;
        for(int i = 0; i < 6; i++)
        {
            if (i < planeCount)
            {
                scores[i].nameText.enabled = true;
                scores[i].killText.enabled = true;
                scores[i].deathText.enabled = true;

                scores[i].nameText.text = PhotonNetwork.PlayerList[i].NickName;
                if (PhotonNetwork.PlayerList[i].IsLocal)
                    scores[i].nameText.color = Color.red;

                scores[i].killText.text = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "kill", 0).ToString();
                scores[i].deathText.text = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "death", 0).ToString();
            }
            else
            {
                scores[i].nameText.enabled = false;
                scores[i].killText.enabled = false;
                scores[i].deathText.enabled = false;
            }
        }
    }

    public void Hide()
    {
        panel.enabled = false;
        title[0].enabled = false;
        title[1].enabled = false;

        for (int i = 0; i < 6; i++)
        {
            scores[i].nameText.enabled = false;
            scores[i].killText.enabled = false;
            scores[i].deathText.enabled = false;
        }
    }
}
