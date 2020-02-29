using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PhotonAI : MonoBehaviour
{
    [SerializeField] UISprite AI_Sprite;
    [SerializeField] Transform spriteRoot;
    [HideInInspector] public ArrayList AI_List = new ArrayList();

    [System.Serializable]
    class AI_Information
    {
        public string nickname;
        public int planeInt;
        public UISprite sprite;
    }

    public void AddAI()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount + AI_List.Count == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            StartCoroutine(FindObjectOfType<MessageShow>().Show("房间人数已满"));
            return;
        }
        
        int index = PhotonNetwork.PlayerListOthers.Length + AI_List.Count;

        AI_Information information = new AI_Information();
        information.planeInt = Random.Range(0, 9);

        AI_List.Add(information);
        
        FindObjectOfType<PhotonRoom>().EnterOrRefreshRoom();

        EnterOrRefreshRoomOfAI();
    }

    public void EnterOrRefreshRoomOfAI()
    {
        for (int i = 0; i < AI_List.Count; i++)
        {
            AI_Information information = AI_List[i] as AI_Information;

            if (information.sprite != null)
                Destroy(information.sprite.gameObject);

            information.sprite = Instantiate(AI_Sprite, spriteRoot);
            UISprite sprite = information.sprite;
            sprite.gameObject.SetActive(true);
            
            information.nickname = string.Format("机器人{0}", i + 1);
            sprite.transform.Find("Name Label").GetComponent<UILabel>().text = information.nickname;

            UISprite roomSprite = FindObjectOfType<PhotonRoom>().usernameSprides[PhotonNetwork.PlayerList.Length + i];
            sprite.leftAnchor = roomSprite.leftAnchor;
            sprite.rightAnchor = roomSprite.rightAnchor;
            sprite.topAnchor = roomSprite.topAnchor;
            sprite.bottomAnchor = roomSprite.bottomAnchor;

            FindObjectOfType<ShowPlane>().ShowAI(PhotonNetwork.PlayerListOthers.Length + i, information.planeInt);
        }
    }

    public void LeftRoomOfAI()
    {
        for (int i = 0; i < AI_List.Count; i++)
        {
            AI_Information information = AI_List[i] as AI_Information;
            Destroy(information.sprite.gameObject);
        }
        
        AI_List.Clear();
    }

    public void RefreshPlaneLabelOnClick(UILabel nameLabel)
    {
        int index = GetIndex(nameLabel);
        AI_Information information = AI_List[index] as AI_Information;
        information.planeInt = Random.Range(0, 9);
        FindObjectOfType<ShowPlane>().DestroyAI(PhotonNetwork.PlayerListOthers.Length + index);
        FindObjectOfType<ShowPlane>().ShowAI(PhotonNetwork.PlayerListOthers.Length + index, information.planeInt);
    }

    public void KickLabelOnClick(UILabel nameLabel)
    {
        int index = GetIndex(nameLabel);
        Destroy((AI_List[index] as AI_Information).sprite.gameObject);
        AI_List.RemoveAt(index);
        
        FindObjectOfType<PhotonRoom>().EnterOrRefreshRoom();

        EnterOrRefreshRoomOfAI();
    }

    int GetIndex(UILabel nameLabel)
    {
        for (int i = 0; i < AI_List.Count; i++)
        {
            AI_Information information = AI_List[i] as AI_Information;
            if (nameLabel.text == information.nickname)
            {
                return i;
            }
        }

        return 0;
    }
}