using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PlaneAttack : MonoBehaviourPun, IAttack
{
    [SerializeField] private Camera planeCamera;

    // Start is called before the first frame update
    private void Start()
    {
        if (!photonView.IsMine)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            return;
        }

        PhotonNetwork.LocalPlayer.SetProperties("HP", 100);
    }

    public void Attack(Player player, Transform target)
    {
        if (!photonView.IsMine) return;

        int randomAttack = Random.Range(5, 15);

        int totalHp = (int) player.GetProperties("HP", 100);
        bool invincible = (bool) player.GetProperties("invincible", false);

        if (totalHp <= 0 || invincible)
            return;

        totalHp -= randomAttack;
        player.SetProperties("HP", totalHp);

        StartCoroutine(ShowSight(target));

        FindObjectOfType<PhotonGame>().photonView.RPC("PlayAudio", player, 2);

        if (totalHp <= 0)
        {
            StartCoroutine(ShowKillImage(target));

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
        int totalHp = targetProperty.HP;
        bool invincible = targetProperty.isInvincible;

        if (totalHp <= 0 || invincible)
            return;

        totalHp -= randomAttack;
        targetProperty.HP = totalHp;

        StartCoroutine(ShowSight(target));

        target.GetComponent<AudioPlayer>().PlayAudio(2);

        if (totalHp <= 0)
        {
            StartCoroutine(ShowKillImage(target));

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
        FindObjectOfType<PhotonGame>().SightImage.sprite = FindObjectOfType<PhotonGame>().SightSprites[1];
        FindObjectOfType<PhotonGame>().SightImage.rectTransform.position =
            planeCamera.WorldToScreenPoint(target.position);

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<PhotonGame>().SightImage.sprite = FindObjectOfType<PhotonGame>().SightSprites[0];
        FindObjectOfType<PhotonGame>().SightImage.rectTransform.position =
            new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    private IEnumerator ShowKillImage(Transform target)
    {
        FindObjectOfType<PhotonGame>().KillImage.enabled = true;

        yield return new WaitForSeconds(1.5f);

        FindObjectOfType<PhotonGame>().KillImage.enabled = false;
    }
}