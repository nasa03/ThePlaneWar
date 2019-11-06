using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlaneAttack : MonoBehaviourPunCallbacks
{
    [SerializeField] Image HP_Image;
    [SerializeField] Image sight_Image;
    [SerializeField] Sprite[] sight_Sprites = new Sprite[3];
    bool isKilled = false;
    bool isSuiside = false;

    // Start is called before the first frame update
    void Start()
    {
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "HP", 100);
    }

    public void Attack(Player player)
    {
        if (!photonView.IsMine)
            return;

        int randomAttack = Random.Range(5, 15);

        int totalHP = (int)CustomProperties.GetProperties(player, "HP", 100);
        bool invincible = (bool)CustomProperties.GetProperties(player, "invincible", false);

        if (totalHP <= 0 || invincible)
            return;

        totalHP -= randomAttack;
        CustomProperties.SetProperties(player, "HP", totalHP);

        StartCoroutine(ShowSight());

        if (totalHP <= 0)
        {
            StartCoroutine(ShowKillSight());

            int kill = (int)CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "kill", 0);
            kill++;
            CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "kill", kill);

            GetComponent<AudioSource>().Play();
            photonView.RPC("AddAttackMessage", RpcTarget.All, string.Format("{0}击杀了{1}", PhotonNetwork.LocalPlayer.NickName, player.NickName));
            photonView.RPC("Dead", player);
        }
    }

    public void Suiside(Player player)
    {
        if (!photonView.IsMine)
            return;

        if (isSuiside)
            return;

        isSuiside = true;

        photonView.RPC("AddAttackMessage", RpcTarget.All, string.Format("{0}自杀了", player.NickName));
        photonView.RPC("Dead", player);

        StartCoroutine(WaitForSuiside());
    }

    IEnumerator ShowSight()
    {
        if (!isKilled)
            sight_Image.sprite = sight_Sprites[1];

        yield return new WaitForSeconds(0.5f);

        if (!isKilled)
            sight_Image.sprite = sight_Sprites[0];
    }

    IEnumerator ShowKillSight()
    {
        isKilled = true;

        sight_Image.sprite = sight_Sprites[2];
        yield return new WaitForSeconds(2.0f);
        sight_Image.sprite = sight_Sprites[0];

        isKilled = false;
    }

    IEnumerator WaitForSuiside()
    {
        yield return new WaitForSeconds(1.0f);
        isSuiside = false;
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        if (target == PhotonNetwork.LocalPlayer)
        {
            HP_Image.fillAmount = (float)((int)CustomProperties.GetProperties(target, "HP", 100) / 100.0);
        }
    }
}
