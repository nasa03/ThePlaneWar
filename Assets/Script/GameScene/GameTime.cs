using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameTime : MonoBehaviourPun
{
    [SerializeField] private Text text;
    private int _time = 600;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ShowTime());
    }

    private IEnumerator ShowTime()
    {
        while (_time >= 0)
        {
            int minutes = _time / 60;
            int seconds = _time % 60;

            string secondsStr;
            if (seconds < 10)
                secondsStr = "0" + seconds;
            else
                secondsStr = seconds.ToString();

            text.text = $"{minutes}:{secondsStr}";
            yield return new WaitForSeconds(1.0f);
            _time--;

            if (_time == 0 && PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                photonView.RPC("GameOver", RpcTarget.All);
            }
        }
    }
}
