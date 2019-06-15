//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlaneController : MonoBehaviourPunCallbacks
{
    [SerializeField] Image HP_Image;
    [SerializeField] Image sight_Image;
    [SerializeField] Sprite[] sight_Sprites = new Sprite[2];

    // Start is called before the first frame update
    void Start()
    {
        setProperties(PhotonNetwork.LocalPlayer, "HP", 100);
    }

    public void Attack(Collider collider)
    {
        Player player = collider.gameObject.GetComponent<PhotonView>().Controller;

        int totalHP = (int)getProperties(player, "HP");
        setProperties(player, "HP", totalHP - 20);

        StartCoroutine(ShowSight());
    }

    void setProperties(Player player,string key,object value)
    {
        Hashtable keyValues = player.CustomProperties;

        if (keyValues.ContainsKey(key))
        {
            keyValues[key] = value;
        }
        else
        {
            keyValues.Add(key, value);
        }

        player.SetCustomProperties(keyValues);
        
    }

    object getProperties(Player player,string key)
    {
        Hashtable keyValues = player.CustomProperties;

        if (keyValues.ContainsKey(key))
        {
            return keyValues[key];
        }
        else
        {
            return null;
        }
    }

    System.Collections.IEnumerator ShowSight()
    {
        sight_Image.sprite = sight_Sprites[1];
        yield return new WaitForSeconds(0.5f);
        sight_Image.sprite = sight_Sprites[0];
    }

    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);

        if (target == PhotonNetwork.LocalPlayer)
        {
            HP_Image.fillAmount = (float)((int)getProperties(target, "HP") / 100.0);
        }
    }
}
