using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameOver : MonoBehaviour {
    
    [SerializeField] private PlaneInformation[] planeInformation = new PlaneInformation[6];

    [Serializable]
    private class PlaneInformation
    {
        public GameObject obj;
        public UILabel nameLabel;
        public UILabel killLabel;
        public UILabel deathLabel;
    }

    public void Show()
    {
        int planeCount = PhotonNetwork.PlayerList.Length;
        for (int i = 0; i < 6; i++)
        {
            if (i < planeCount + Global.aiPlaneScores.Count)
            {
                if (i < planeCount)
                {
                    planeInformation[i].obj.SetActive(true);

                    planeInformation[i].nameLabel.text = PhotonNetwork.PlayerList[i].NickName;
                    if (PhotonNetwork.PlayerList[i].IsLocal)
                        planeInformation[i].nameLabel.color = Color.yellow;
                    if (PhotonNetwork.PlayerList[i].IsMasterClient)
                        planeInformation[i].nameLabel.color = Color.red;

                    planeInformation[i].killLabel.text =
                        PhotonNetwork.PlayerList[i].GetProperties("kill", 0).ToString();
                    planeInformation[i].deathLabel.text =
                        PhotonNetwork.PlayerList[i].GetProperties("death", 0).ToString();
                }
                else
                {
                    planeInformation[i].obj.SetActive(true);

                    Global.AIPlaneScores aiPlaneScores = (Global.AIPlaneScores) Global.aiPlaneScores[i - planeCount];
                    planeInformation[i].nameLabel.text = aiPlaneScores.name;
                    planeInformation[i].nameLabel.color = Color.green;

                    planeInformation[i].killLabel.text = aiPlaneScores.kill.ToString();
                    ;
                    planeInformation[i].deathLabel.text = aiPlaneScores.death.ToString();
                }
            }
            else
            {
                planeInformation[i].obj.SetActive(false);
            }
        }

        ResetInformation();
    }

    public void ResetInformation()
    {
        PhotonNetwork.LocalPlayer.SetProperties("kill", 0);
        PhotonNetwork.LocalPlayer.SetProperties("death", 0);

        Global.aiPlaneScores.Clear();
    }
}
