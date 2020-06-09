using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PhotonGame : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject[] planePrefabs;
    [SerializeField] Transform[] groundRunwayPosition;
    [SerializeField] GameObject explosionParticleSystem;
    [SerializeField] GameObject timeBar;
    [SerializeField] GameObject sightImage;
    [SerializeField] Text timeText;
    [SerializeField] Image timeImage;
    GameObject localPlane;
    bool reborn = false;
    bool invincible = false;
    bool reconnected = false;
    float time = 0.0f;
    int maxTime = 0;

    public bool Reborn => reborn;
    
    public Transform[] GroundRunwayPosition => groundRunwayPosition;

    // Start is called before the first frame update
    void Start()
    {
        localPlane = PhotonNetwork.Instantiate(planePrefabs[Global.totalPlaneInt].name,
            groundRunwayPosition[Global.totalPlayerInt].position + new Vector3(0, 15, 0), Quaternion.identity);

        StartCoroutine(InvincibleStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            timeImage.fillAmount = time / maxTime;
            time -= Time.deltaTime;
        }
        else
        {
            if (reborn)
            {
                RebornEndOrReconnect();
            }

            if (invincible)
            {
                InvincibleEnd();
            }

            if (reconnected)
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }

    public void OnExitButtonClick()
    {
        Global.returnState = Global.ReturnState.exitGame;
        PhotonNetwork.Destroy(localPlane);
        SceneManager.LoadScene("StartScene");
    }

    [PunRPC]
    public void GameOver()
    {
        Global.returnState = Global.ReturnState.gameOver;
        PhotonNetwork.Destroy(localPlane);
        SceneManager.LoadScene("StartScene");
    }

    [PunRPC]
    public void Dead()
    {
        GameObject explosion = PhotonNetwork.Instantiate(explosionParticleSystem.name, localPlane.transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioPlayer>().PlayAudio(3);
        
        localPlane.GetComponent<MeshRenderer>().enabled = false;
        ParticleSystem[] particleSystems = localPlane.GetComponentsInChildren<ParticleSystem>();
        foreach (var items in particleSystems)
        {
            items.Stop();
        }

        invincible = false;

        StartCoroutine(RebornStart());

        int death = (int)CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "death", 0);
        death++;
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "death", death);

        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "HP", 100);
    }

    IEnumerator RebornStart()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.Destroy(localPlane);

        mainCamera.enabled = true;
        timeBar.SetActive(true);
        sightImage.SetActive(false);

        time = 10.0f;
        maxTime = 10;
        timeText.text = "重生";

        yield return new WaitForSeconds(1.0f);
        reborn = true;
    }

    public void RebornEndOrReconnect()
    {
        mainCamera.enabled = false;
        timeBar.SetActive(false);
        sightImage.SetActive(true);

        localPlane = PhotonNetwork.Instantiate(planePrefabs[Global.totalPlaneInt].name, groundRunwayPosition[Global.totalPlayerInt].position + new Vector3(0, 15, 0), Quaternion.identity);

        reborn = false;
        reconnected = false;

        StartCoroutine(InvincibleStart());
    }

    IEnumerator InvincibleStart()
    {
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "invincible", true);
        timeBar.SetActive(true);
        time = 20.0f;
        maxTime = 20;
        timeText.text = "无敌状态";

        yield return new WaitForSeconds(1.0f);
        invincible = true;
    }

    void InvincibleEnd()
    {
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "invincible", false);
        timeBar.SetActive(false);

        invincible = false;
    }

    public void Disconnect()
    {
        if (reconnected)
            return;

        mainCamera.enabled = true;
        timeBar.SetActive(true);
        sightImage.SetActive(false);

        time = 60.0f;
        maxTime = 60;
        timeText.text = "正在重连";

        reconnected = true;
    }
}