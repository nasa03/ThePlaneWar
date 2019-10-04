using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameTime : MonoBehaviourPun
{
    [SerializeField] Text text;
    int time = 600;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowTime());
    } 

    IEnumerator ShowTime()
    {
        while (time >= 0)
        {
            int minutes = time / 60;
            int seconds = time % 60;

            string secondsStr;
            if (seconds < 10)
                secondsStr = "0" + seconds;
            else
                secondsStr = seconds.ToString();

            text.text = string.Format("{0}:{1}", minutes, secondsStr);
            yield return new WaitForSeconds(1.0f);
            time--;

            if (time == 0 && PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                photonView.RPC("GameOver", RpcTarget.All);
            }
        }
    }
}
