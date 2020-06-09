using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameOver : MonoBehaviour
{
    [System.Serializable]
    class PlaneInformation
    {
        public GameObject obj;
        public UILabel nameLabel;
        public UILabel killLabel;
        public UILabel deathLabel;
    }

    [SerializeField] PlaneInformation[] planeInformation = new PlaneInformation[6];

    public void Show()
    {
        int planeCount = PhotonNetwork.PlayerList.Length;
        for (int i = 0; i < 6; i++)
        {
            if (i < planeCount)
            {
                planeInformation[i].obj.SetActive(true);

                planeInformation[i].nameLabel.text = PhotonNetwork.PlayerList[i].NickName;
                if (PhotonNetwork.PlayerList[i].IsLocal)
                    planeInformation[i].nameLabel.color = Color.red;

                planeInformation[i].killLabel.text = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "kill", 0).ToString();
                planeInformation[i].deathLabel.text = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "death", 0).ToString();
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
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "kill", 0);
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "death", 0);
    }
}
