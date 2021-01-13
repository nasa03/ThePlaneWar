using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlaneSuicide : MonoBehaviourPun
{
    private bool _isSuicide = false;

    // Update is called once per frame
    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Suicide") && !_isSuicide)
            StartCoroutine(Suicide());
    }

    private IEnumerator Suicide()
    {
        if (!photonView.IsMine)
            yield break;

        if (FindObjectOfType<PhotonGame>().Reborn)
            yield break;

        _isSuicide = true;

        FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
            $"{PhotonNetwork.LocalPlayer.NickName}自杀了");
        FindObjectOfType<PhotonGame>().photonView.RPC("Dead", PhotonNetwork.LocalPlayer);

        yield return new WaitForSeconds(2.0f);

        _isSuicide = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FX") && !_isSuicide)
            StartCoroutine(Suicide());
    }
}
