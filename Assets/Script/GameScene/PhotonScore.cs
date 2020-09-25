﻿using System;
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
        ArrayList aiPlaneList = FindObjectOfType<PhotonGameAI>().AI_Plane_List;
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
                            scores[i].scoreCamera = planeObj.transform.Find("ShakeCamera")
                                .Find("MultipurposeCameraRig").gameObject;
                        }
                    }
                    if (PhotonNetwork.PlayerList[i].IsLocal)
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
        FindObjectOfType<PhotonGame>().LocalPlane.GetComponent<CameraActive>().Camera.SetActive(false);
        scores[index].scoreCamera.SetActive(true);
    }

    public void Hide(int index)
    {
        scores[index].scoreCamera.SetActive(false);
        FindObjectOfType<PhotonGame>().LocalPlane.GetComponent<CameraActive>().Camera.SetActive(true);
    }
}
