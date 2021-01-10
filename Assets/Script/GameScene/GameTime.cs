using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityStandardAssets.CrossPlatformInput;

public class GameTime : MonoBehaviourPun
{
    [SerializeField] private Text text;
    [SerializeField] private GameObject gameOverButton;
    private int _time = 0;
    private const int MAXTime = 600;

    // Start is called before the first frame update
    private void Start()
    {
        _time = MAXTime;
        gameOverButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("GameOver") && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GameOver", RpcTarget.All);
        }
    }

    public IEnumerator ShowTime()
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
            _time = Mathf.Clamp(--_time, 0, MAXTime);

            if (_time == 0 && PhotonNetwork.IsMasterClient && photonView.IsMine)
            {
                photonView.RPC("GameOver", RpcTarget.All);
            }
        }
    }
}
