using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PlaneAttack : MonoBehaviourPunCallbacks
{
    [SerializeField] Camera planeCamera;
    [SerializeField] Sprite[] sight_Sprites = new Sprite[3];
    PhotonGame photonGame;
    PhotonView gameView;
    bool isKilled = false;

    // Start is called before the first frame update
    void Start()
    {
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "HP", 100);
        photonGame.SightImage.rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        photonGame.HpLerpImage.fillAmount = Mathf.Lerp(
            (float) ((int) CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "HP", 100) / 100.0),
            photonGame.HpLerpImage.fillAmount, 0.95f);
    }

    private void Awake()
    {
        photonGame = FindObjectOfType<PhotonGame>();
        gameView = photonGame.GetComponent<PhotonView>();
    }

    public void Attack(Player player,Transform target)
    {
        if (!photonView.IsMine) return;

        int randomAttack = Random.Range(5, 15);

        int totalHP = (int) CustomProperties.GetProperties(player, "HP", 100);
        bool invincible = (bool) CustomProperties.GetProperties(player, "invincible", false);

        if (totalHP <= 0 || invincible)
            return;

        totalHP -= randomAttack;
        CustomProperties.SetProperties(player, "HP", totalHP);

        StartCoroutine(ShowSight(target));

        gameView.RPC("PlayAudio", player, 2);

        if (totalHP <= 0)
        {
            StartCoroutine(ShowKillSight(target));

            int kill = (int) CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "kill", 0);
            kill++;
            CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "kill", kill);

            GetComponent<AudioSource>().Play();
            gameView.RPC("AddAttackMessage", RpcTarget.All,
                string.Format("{0}击杀了{1}", PhotonNetwork.LocalPlayer.NickName, player.NickName));
            gameView.RPC("Dead", player);
        }
    }
    
    public void AttackAI(Transform target)
    {
        if (!photonView.IsMine) return;
        
        int randomAttack = Random.Range(5, 15);

        AIProperty targetProperty = target.GetComponent<AIProperty>();
        int totalHP = targetProperty.HP;

        if (totalHP <= 0)
            return;

        totalHP -= randomAttack;
        targetProperty.HP = totalHP;

        StartCoroutine(ShowSight(target));

        target.GetComponent<AudioPlayer>().PlayAudio(2);

        if (totalHP <= 0)
        {
            StartCoroutine(ShowKillSight(target));

            int kill = (int) CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "kill", 0);
            kill++;
            CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "kill", kill);

            GetComponent<AudioSource>().Play();
            gameView.RPC("AddAttackMessage", RpcTarget.All,
                string.Format("{0}击杀了{1}", PhotonNetwork.LocalPlayer.NickName, targetProperty.Name));
            target.GetComponent<AIAttack>().Dead();
        }
    }

    IEnumerator ShowSight(Transform target)
    {
        if (!isKilled)
        {
            photonGame.SightImage.sprite = sight_Sprites[1];
            photonGame.SightImage.rectTransform.position = planeCamera.WorldToScreenPoint(target.position);
        }
        
        yield return new WaitForSeconds(0.5f);

        if (!isKilled)
        {
            photonGame.SightImage.sprite = sight_Sprites[0];
            photonGame.SightImage.rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
    }

    IEnumerator ShowKillSight(Transform target)
    {
        isKilled = true;

        photonGame.SightImage.sprite = sight_Sprites[2];
        photonGame.SightImage.rectTransform.position = planeCamera.WorldToScreenPoint(target.position);
        
        yield return new WaitForSeconds(2.0f);
        
        photonGame.SightImage.sprite = sight_Sprites[0];
        photonGame.SightImage.rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        
        isKilled = false;
    }


    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        if (target == PhotonNetwork.LocalPlayer)
        {
            photonGame.HPImage.fillAmount = (float) ((int) CustomProperties.GetProperties(target, "HP", 100) / 100.0);
        }
    }
    
}
