using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PlaneAttack : MonoBehaviourPun
{
    [SerializeField] private Camera planeCamera;
    private bool _isKilled = false;

    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.LocalPlayer.SetProperties("HP", 100);
    }

    public void Attack(Player player, Transform target)
    {
        if (!photonView.IsMine) return;

        int randomAttack = Random.Range(5, 15);

        int totalHP = (int) player.GetProperties("HP", 100);
        bool invincible = (bool) player.GetProperties("invincible", false);

        if (totalHP <= 0 || invincible)
            return;

        totalHP -= randomAttack;
        player.SetProperties("HP", totalHP);

        StartCoroutine(ShowSight(target));

        FindObjectOfType<PhotonGame>().photonView.RPC("PlayAudio", player, 2);

        if (totalHP <= 0)
        {
            StartCoroutine(ShowKillSight(target));

            int kill = (int) PhotonNetwork.LocalPlayer.GetProperties("kill", 0);
            kill++;
            PhotonNetwork.LocalPlayer.SetProperties("kill", kill);

            GetComponent<AudioSource>().Play();
            FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
                $"{PhotonNetwork.LocalPlayer.NickName}击杀了{player.NickName}");
            FindObjectOfType<PhotonGame>().photonView.RPC("Dead", player);
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

            int kill = (int) PhotonNetwork.LocalPlayer.GetProperties("kill", 0);
            kill++;
            PhotonNetwork.LocalPlayer.SetProperties("kill", kill);

            GetComponent<AudioSource>().Play();
            FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
                $"{PhotonNetwork.LocalPlayer.NickName}击杀了{target.name}");
            FindObjectOfType<PhotonGame>().photonView.RPC("DeadAI", RpcTarget.All, target.name);
        }
    }

    private IEnumerator ShowSight(Transform target)
    {
        if (!_isKilled)
        {
            FindObjectOfType<PhotonGame>().SightImage.sprite = FindObjectOfType<PhotonGame>().SightSprites[1];
            FindObjectOfType<PhotonGame>().SightImage.rectTransform.position =
                planeCamera.WorldToScreenPoint(target.position);
        }

        yield return new WaitForSeconds(0.5f);

        if (!_isKilled)
        {
            FindObjectOfType<PhotonGame>().SightImage.sprite = FindObjectOfType<PhotonGame>().SightSprites[0];
            FindObjectOfType<PhotonGame>().SightImage.rectTransform.position =
                new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
    }

    private IEnumerator ShowKillSight(Transform target)
    {
        _isKilled = true;

        FindObjectOfType<PhotonGame>().SightImage.sprite = FindObjectOfType<PhotonGame>().SightSprites[2];
        FindObjectOfType<PhotonGame>().SightImage.rectTransform.position =
            planeCamera.WorldToScreenPoint(target.position);

        yield return new WaitForSeconds(2.0f);

        FindObjectOfType<PhotonGame>().SightImage.sprite = FindObjectOfType<PhotonGame>().SightSprites[0];
        FindObjectOfType<PhotonGame>().SightImage.rectTransform.position =
            new Vector3(Screen.width / 2, Screen.height / 2, 0);

        _isKilled = false;
    }
}