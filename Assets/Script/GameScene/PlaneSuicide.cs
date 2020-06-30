using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlaneSuicide : MonoBehaviourPun
{
    PhotonView gameView;
    bool isSuicide = false;

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Suicide") && !isSuicide)
        {
            StartCoroutine(Suicide());
        }
    }

    private void Awake()
    {
        gameView = FindObjectOfType<PhotonGame>().GetComponent<PhotonView>();
    }

    IEnumerator Suicide()
    {
        if (!photonView.IsMine)
            yield break;

        if (FindObjectOfType<PhotonGame>().Reborn)
            yield break;

        isSuicide = true;
        
        gameView.RPC("AddAttackMessage", RpcTarget.All, string.Format("{0}自杀了", PhotonNetwork.LocalPlayer.NickName));
        gameView.RPC("Dead", PhotonNetwork.LocalPlayer);
        
        yield return new WaitForSeconds(2.0f);

        isSuicide = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "FX" && !isSuicide)
        {
            StartCoroutine(Suicide());
        }
    }
}
