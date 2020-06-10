using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PlaneAttack : MonoBehaviourPun
{
    [SerializeField] Sprite[] sight_Sprites = new Sprite[3]; 
    PhotonGame photonGame;
    PhotonView gameView;
    bool isKilled = false;

    // Start is called before the first frame update
    void Start()
    {
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "HP", 100);
    }
    
    // Update is called once per frame
    void Update()
    {
        photonGame.HPImage.fillAmount = Mathf.Lerp(
            (float) ((int) CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "HP", 100) / 100.0),
            photonGame.HPImage.fillAmount, 0.95f);
    }
    
    private void Awake()
    {
        photonGame = FindObjectOfType<PhotonGame>();
        gameView = photonGame.GetComponent<PhotonView>();
    }

    [PunRPC]
    public void Attack(Player player)
    {
        if (!photonView.IsMine)
            return;

        int randomAttack = Random.Range(5, 15);

        int totalHP = (int) CustomProperties.GetProperties(player, "HP", 100);
        bool invincible = (bool) CustomProperties.GetProperties(player, "invincible", false);

        if (totalHP <= 0 || invincible)
            return;

        totalHP -= randomAttack;
        CustomProperties.SetProperties(player, "HP", totalHP);

        StartCoroutine(ShowSight());

        gameView.RPC("PlayAudio", player, 2);

        if (totalHP <= 0)
        {
            StartCoroutine(ShowKillSight());

            int kill = (int) CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "kill", 0);
            kill++;
            CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "kill", kill);

            GetComponent<AudioSource>().Play();
            gameView.RPC("AddAttackMessage", RpcTarget.All,
                string.Format("{0}击杀了{1}", PhotonNetwork.LocalPlayer.NickName, player.NickName));
            gameView.RPC("Dead", player);
        }
    }

    IEnumerator ShowSight()
    {
        if (!isKilled)
            photonGame.SightImage.sprite = sight_Sprites[1];

        yield return new WaitForSeconds(0.5f);

        if (!isKilled)
            photonGame.SightImage.sprite = sight_Sprites[0];
    }

    IEnumerator ShowKillSight()
    {
        isKilled = true;

        photonGame.SightImage.sprite = sight_Sprites[2];
        yield return new WaitForSeconds(2.0f);
        photonGame.SightImage.sprite = sight_Sprites[0];

        isKilled = false;
    }
}
