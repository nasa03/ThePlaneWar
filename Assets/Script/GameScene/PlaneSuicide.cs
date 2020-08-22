using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlaneSuicide : MonoBehaviourPun
{
    bool isSuicide = false;

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Suicide") && !isSuicide)
        {
            StartCoroutine(Suicide());
        }
    }

    IEnumerator Suicide()
    {
        if (!photonView.IsMine)
            yield break;

        if (FindObjectOfType<PhotonGame>().Reborn)
            yield break;

        isSuicide = true;

        FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
            string.Format("{0}自杀了", PhotonNetwork.LocalPlayer.NickName));
        FindObjectOfType<PhotonGame>().photonView.RPC("Dead", PhotonNetwork.LocalPlayer);

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
