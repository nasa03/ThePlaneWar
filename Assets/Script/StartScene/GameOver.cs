using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameOver : MonoBehaviour
{
    [System.Serializable]
    class PlaneInfomation
    {
        public GameObject obj;
        public UILabel nameLabel;
        public UILabel killLabel;
        public UILabel deathLabel;
    }

    [SerializeField] PlaneInfomation[] planeInfomations = new PlaneInfomation[6];

    public void Show()
    {
        int planeCount = PhotonNetwork.PlayerList.Length;
        for (int i = 0; i < 6; i++)
        {
            if (i < planeCount)
            {
                planeInfomations[i].obj.SetActive(true);

                planeInfomations[i].nameLabel.text = PhotonNetwork.PlayerList[i].NickName;
                if (PhotonNetwork.PlayerList[i].IsLocal)
                    planeInfomations[i].nameLabel.color = Color.red;

                planeInfomations[i].killLabel.text = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "kill", 0).ToString();
                planeInfomations[i].deathLabel.text = CustomProperties.GetProperties(PhotonNetwork.PlayerList[i], "death", 0).ToString();
            }
            else
            {
                planeInfomations[i].obj.SetActive(false);
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
