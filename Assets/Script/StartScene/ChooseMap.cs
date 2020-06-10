using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChooseMap : MonoBehaviourPunCallbacks
{
    [SerializeField] UISprite mapSprite;
    [SerializeField] UILabel mapLabel;
    [SerializeField] UIButton[] mapButtons; 
    int index = 1;

    public int Index => index;

    public void LastMap()
    {
        List<UISpriteData> list = mapSprite.atlas.spriteList;
        int mapIndex = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].name == mapSprite.spriteName)
            {
                mapIndex = i;
                break;
            }
        }

        if (mapIndex == 0)
        {
            return;
        }

        mapIndex--;
        
        photonView.RPC("SetMap", RpcTarget.All, mapIndex);
    }

    public void NextMap()
    {
        List<UISpriteData> list = mapSprite.atlas.spriteList;
        int mapIndex = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].name == mapSprite.spriteName)
            {
                mapIndex = i;
                break;
            }
        }

        if (mapIndex == list.Count - 1)
        {
            return;
        }

        mapIndex++;

        photonView.RPC("SetMap", RpcTarget.All, mapIndex);
    }

    [PunRPC]
    void SetMap(int index)
    {
        mapSprite.spriteName = mapSprite.atlas.spriteList[index].name;
        mapLabel.text = "Map " + (index + 1);
        this.index = (index + 1);
    }
    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            foreach (UIButton button in mapButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (UIButton button in mapButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            foreach (UIButton button in mapButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (UIButton button in mapButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
