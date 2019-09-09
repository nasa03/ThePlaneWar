using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PhotonGame : MonoBehaviour {
    [SerializeField] GameObject[] planePrefabs;
    [SerializeField] Transform[] groundRunwayPosotion;
    [SerializeField] GameObject explosionParticleSystem;
    [SerializeField] Image RebornImage;
    [SerializeField] Text RebornText;
    GameObject localPlane;

    // Start is called before the first frame update
    void Start()
    {
        localPlane = PhotonNetwork.Instantiate(planePrefabs[Global.totalPlaneInt].name, groundRunwayPosotion[Global.totalPlayerInt].position + new Vector3(0, 10, 0), Quaternion.identity);

        Global.inGame = true;
    }

    public void OnExitButtonClick()
    {
        PhotonNetwork.Destroy(localPlane);
        SceneManager.LoadScene("StartScene");
    }

    [PunRPC]
    public void Dead()
    {
        GameObject explosion = PhotonNetwork.Instantiate(explosionParticleSystem.name, localPlane.transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();

        PhotonNetwork.Destroy(localPlane);
        StartCoroutine(Reborn());

        int death = (int)CustomProperties.GetProperties(PhotonNetwork.LocalPlayer, "death", 0);
        death++;
        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "death", death);

        CustomProperties.SetProperties(PhotonNetwork.LocalPlayer, "HP", 100);
    }

    IEnumerator Reborn()
    {
        RebornImage.enabled = true;
        RebornText.enabled = true;
        int time = 10;
        do
        {
            RebornText.text = string.Format("还有{0}秒重生", time);
            yield return new WaitForSeconds(1.0f);
            time--;
        } while (time > 0);

        RebornImage.enabled = false;
        RebornText.enabled = false;

        localPlane = PhotonNetwork.Instantiate(planePrefabs[Global.totalPlaneInt].name, groundRunwayPosotion[Global.totalPlayerInt].position + new Vector3(0, 10, 0), Quaternion.identity);
    }
}
