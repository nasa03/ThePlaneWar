using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PhotonAI : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIButton addAIButton;
    [SerializeField] private UISprite[] aiSprites = new UISprite[5];

    public ArrayList AIList { get; } = new ArrayList();

    public void AddAI()
    {
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers == 0 ? 6 : PhotonNetwork.CurrentRoom.MaxPlayers;

        if (PhotonNetwork.CurrentRoom.PlayerCount + AIList.Count == maxPlayers)
        {
            StartCoroutine(FindObjectOfType<MessageShow>().Show("房间人数已满"));
            return;
        }

        AIList.Add(Random.Range(0, 9));

        PhotonNetwork.CurrentRoom.SetProperties("ai_List", AIList.ToArray());

        photonView.RPC("EnterOrRefreshRoom", RpcTarget.All);
        photonView.RPC("EnterOrRefreshRoomOfAI", RpcTarget.All);
    }

    public void RefreshAI(UILabel nameLabel)
    {
        int index = GetIndex(nameLabel);
        AIList[index] = Random.Range(0, 9);

        PhotonNetwork.CurrentRoom.SetProperties("ai_List", AIList.ToArray());

        photonView.RPC("EnterOrRefreshRoomOfAI", RpcTarget.All);
    }

    public void KickAI(UILabel nameLabel)
    {
        int index = GetIndex(nameLabel);
        AIList.RemoveAt(index);

        PhotonNetwork.CurrentRoom.SetProperties("ai_List", AIList.ToArray());

        photonView.RPC("EnterOrRefreshRoom", RpcTarget.All);
        photonView.RPC("EnterOrRefreshRoomOfAI", RpcTarget.All);
    }

    [PunRPC]
    public void EnterOrRefreshRoomOfAI()
    {
        object[] list = (object[]) PhotonNetwork.CurrentRoom.GetProperties("ai_List");
        AIList.Clear();
        
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers == 0 ? 6 : PhotonNetwork.CurrentRoom.MaxPlayers;
        
        if (list != null)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (i + playerCount + 1 <= maxPlayers)
                {
                    AIList.Add(list[i]);
                }
            }
            
            PhotonNetwork.CurrentRoom.SetProperties("ai_List", AIList.ToArray());
        }

        addAIButton.isEnabled = PhotonNetwork.LocalPlayer.IsMasterClient;

        foreach (var sprite in aiSprites)
        {
            sprite.gameObject.SetActive(false);
        }

        for (int i = 0; i < AIList.Count; i++)
        {
            UISprite sprite = aiSprites[PhotonNetwork.PlayerListOthers.Length + i];
            sprite.gameObject.SetActive(true);

            sprite.transform.Find("Name Label").GetComponent<UILabel>().text = $"机器人{i + 1}";
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
            FindObjectOfType<ShowPlane>().ShowAI(PhotonNetwork.PlayerListOthers.Length + i, (int) AIList[i]);
        }
    }

    public void LeftRoomOfAI()
    {
        AIList.Clear();
    }

    private int GetIndex(UILabel nameLabel)
    {
        for (int i = 0; i < AIList.Count; i++)
        {
            if (nameLabel.text == $"机器人{i + 1}")
            {
                return i;
            }
        }

        return 0;
    }
}