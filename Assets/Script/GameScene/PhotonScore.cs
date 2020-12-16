using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PhotonScore : MonoBehaviourPunCallbacks
{
    [SerializeField] private Score title;
    [SerializeField] private Score[] scores = new Score[6];
    
    [Serializable]
    private class Score
    {
        public Image scoreImage;
        public Text scoreText;
        public GameObject scoreCamera;
    }

    // Update is called once per frame
    private void Update()
    {
        title.scoreImage.enabled = true;
        title.scoreText.enabled = true;

        int planeCount = PhotonNetwork.PlayerList.Length;
        ArrayList aiPlaneList = FindObjectOfType<PhotonGameAI>().aiPlaneList;
        GameObject[] planeObjs = GameObject.FindGameObjectsWithTag("Plane");
        for (int i = 0; i < 6; i++)
        {
            if (i < planeCount + aiPlaneList.Count)
            {
                if (i < planeCount)
                {
                    scores[i].scoreImage.enabled = true;
                    scores[i].scoreText.enabled = true;

                    string name = PhotonNetwork.PlayerList[i].NickName;
                    string kill = PhotonNetwork.PlayerList[i].GetProperties("kill", 0).ToString();
                    string dead = PhotonNetwork.PlayerList[i].GetProperties("death", 0).ToString();
                    scores[i].scoreText.text = $"{name} {kill}/{dead}";
                    foreach (GameObject planeObj in planeObjs)
                    {
                        if (planeObj.GetComponent<PhotonView>().Controller.NickName == name)
                        {
                            scores[i].scoreCamera = planeObj.GetComponent<CameraActive>().Camera;
                        }
                    }
                    if (PhotonNetwork.PlayerList[i].IsLocal)
                        scores[i].scoreText.color = Color.yellow;
                    if (PhotonNetwork.PlayerList[i].IsMasterClient)
                        scores[i].scoreText.color = Color.red;
                }
                else
                {
                    scores[i].scoreImage.enabled = true;
                    scores[i].scoreText.enabled = true;

                    GameObject aiPlane = (GameObject) aiPlaneList[i - planeCount];
                    AIProperty aiProperty = aiPlane.GetComponent<AIProperty>();
                    string name = aiPlane.name;
                    string kill = aiProperty.Kill.ToString();
                    string dead = aiProperty.Death.ToString();
                    scores[i].scoreText.text = $"{name} {kill}/{dead}";
                    scores[i].scoreCamera = aiPlane.transform.Find("ShakeCamera")
                        .Find("MultipurposeCameraRig").gameObject;
                    scores[i].scoreText.color = Color.green;
                }
                
            }
            else
            {
                scores[i].scoreImage.enabled = false;
                scores[i].scoreText.enabled = false;
            }
        }
    }
    
    public void Show(int index)
    {
        Camera mainCamera = FindObjectOfType<PhotonGame>().MainCamera;
        GameObject hideCamera = FindObjectOfType<PhotonGame>().LocalPlane;
        GameObject showCamera = scores[index].scoreCamera;
        if (hideCamera)
        {
            hideCamera.GetComponent<CameraActive>().Camera.SetActive(false);
        }
        else
        {
            mainCamera.enabled = false;
        }

        if (showCamera)
        {
            showCamera.SetActive(true);
        }
        else
        {
            mainCamera.enabled = true;
        }
        
    }

    public void Hide(int index)
    {
        Camera mainCamera = FindObjectOfType<PhotonGame>().MainCamera;
        GameObject showCamera = FindObjectOfType<PhotonGame>().LocalPlane;
        GameObject hideCamera = scores[index].scoreCamera;
        
        if (hideCamera)
        {
            hideCamera.SetActive(false);
        }
        else
        {
            mainCamera.enabled = false;
        }

        if (showCamera)
        {
            showCamera.GetComponent<CameraActive>().Camera.SetActive(true);
        }
        else
        {
            mainCamera.enabled = true;
        }
    }
}
