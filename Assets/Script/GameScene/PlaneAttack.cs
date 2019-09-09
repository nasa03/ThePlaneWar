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

    // Start is called before the first frame update
    void Start()
    {
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "HP", 100);
    }

    public void Attack(Collider collider)
    {
        Player player = collider.gameObject.GetComponent<PhotonView>().Controller;

        int randomAttack = Random.Range(5, 15);

        int totalHP = (int)CustomProperties.GetProperties(player, "HP", 100);

        if (totalHP <= 0)
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
            photonView.RPC("Dead", player);
        }
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

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        if (target == PhotonNetwork.LocalPlayer)
        {
            HP_Image.fillAmount = (float)((int)CustomProperties.GetProperties(target, "HP", 100) / 100.0);
        }
    }
}
