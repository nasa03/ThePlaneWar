using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PhotonAI : MonoBehaviourPunCallbacks
{
    [SerializeField] UIButton AddAIButton;
    [SerializeField] UISprite[] AI_Sprites = new UISprite[5];
    [HideInInspector] public ArrayList AI_List = new ArrayList();

    public void AddAI()
    {
        int maxPlayers;
        if (PhotonNetwork.CurrentRoom.MaxPlayers == 0)
        {
            maxPlayers = 6;
        }
        else
        {
            maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount + AI_List.Count == maxPlayers)

        {
            StartCoroutine(FindObjectOfType<MessageShow>().Show("房间人数已满"));
            return;
        }

        AI_List.Add(Random.Range(0, 9));

        CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, "AI_List", AI_List.ToArray());

        photonView.RPC("EnterOrRefreshRoom", RpcTarget.All);
        photonView.RPC("EnterOrRefreshRoomOfAI", RpcTarget.All);
    }

    public void RefreshAI(UILabel nameLabel)
    {
        int index = GetIndex(nameLabel);
        AI_List[index] = Random.Range(0, 9);

        CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, "AI_List", AI_List.ToArray());

        photonView.RPC("EnterOrRefreshRoomOfAI", RpcTarget.All);
    }

    public void KickAI(UILabel nameLabel)
    {
        int index = GetIndex(nameLabel);
        AI_List.RemoveAt(index);

        CustomProperties.SetProperties(PhotonNetwork.CurrentRoom, "AI_List", AI_List.ToArray());

        photonView.RPC("EnterOrRefreshRoom", RpcTarget.All);
        photonView.RPC("EnterOrRefreshRoomOfAI", RpcTarget.All);
    }

    [PunRPC]
    public void EnterOrRefreshRoomOfAI()
    {
        object[] list = (object[]) CustomProperties.GetProperties(PhotonNetwork.CurrentRoom, "AI_List");
        AI_List.Clear();
        if (list != null)
        {
            for (int i = 0; i < list.Length; i++)
            {
                AI_List.Add(list[i]);
            }
        }

        AddAIButton.isEnabled = PhotonNetwork.LocalPlayer.IsMasterClient;
        
        for (int i = 0; i < AI_Sprites.Length; i++)
        {
            AI_Sprites[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < AI_List.Count; i++)
        {
            UISprite sprite = AI_Sprites[PhotonNetwork.PlayerListOthers.Length + i];
            sprite.gameObject.SetActive(true);

            sprite.transform.Find("Name Label").GetComponent<UILabel>().text = string.Format("机器人{0}", i + 1);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                sprite.gameObject.transform.Find("Refresh Label").gameObject.SetActive(true);
                sprite.gameObject.transform.Find("Kick Label").gameObject.SetActive(true);
            }
            else
            {
                sprite.gameObject.transform.Find("Refresh Label").gameObject.SetActive(false);
                sprite.gameObject.transform.Find("Kick Label").gameObject.SetActive(false);
            }

            FindObjectOfType<ShowPlane>().DestroyAI(PhotonNetwork.PlayerListOthers.Length + i);
            FindObjectOfType<ShowPlane>().ShowAI(PhotonNetwork.PlayerListOthers.Length + i, (int) AI_List[i]);
        }
    }

    public void LeftRoomOfAI()
    {
        AI_List.Clear();
    }

    int GetIndex(UILabel nameLabel)
    {
        for (int i = 0; i < AI_List.Count; i++)
        {
            if (nameLabel.text == string.Format("机器人{0}", i + 1))
            {
                return i;
            }
        }

        return 0;
    }
}