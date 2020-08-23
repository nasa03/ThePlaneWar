using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIAttack : MonoBehaviour
{
    AIProperty aiProperty;

    bool reborn = false;

    float time = 0.0f;
    int maxTime = 0;

    int index;

    public int Index
    {
        set => index = value;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            if (reborn)
            {
                RebornEnd();
            }
        }
    }

    private void Awake()
    {
        aiProperty = GetComponent<AIProperty>();
    }

    public void Attack(Player player, Transform target)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int randomAttack = Random.Range(5, 15);

        int totalHP = (int) CustomProperties.GetProperties(player, "HP", 100);
        bool invincible = (bool) CustomProperties.GetProperties(player, "invincible", false);

        if (totalHP <= 0 || invincible)
            return;

        totalHP -= randomAttack;
        CustomProperties.SetProperties(player, "HP", totalHP);

        FindObjectOfType<PhotonGame>().photonView.RPC("PlayAudio", player, 2);

        if (totalHP <= 0)
        {
            aiProperty.Kill++;

            GetComponent<AudioSource>().Play();
            FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
                string.Format("{0}击杀了{1}", gameObject.name, player.NickName));
            FindObjectOfType<PhotonGame>().photonView.RPC("Dead", player);
        }
    }

    public void AttackAI(Transform target)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int randomAttack = Random.Range(5, 15);

        AIProperty targetProperty = target.GetComponent<AIProperty>();
        int totalHP = targetProperty.HP;

        if (totalHP <= 0)
            return;

        totalHP -= randomAttack;
        targetProperty.HP = totalHP;

        target.GetComponent<AudioPlayer>().PlayAudio(2);

        if (totalHP <= 0)
        {
            aiProperty.Kill++;

            GetComponent<AudioSource>().Play();
            FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
                string.Format("{0}击杀了{1}", gameObject.name, target.name));
            FindObjectOfType<PhotonGame>().photonView.RPC("DeadAI", RpcTarget.All, target.name);
        }
    }

    void Suicide()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        FindObjectOfType<PhotonGame>().photonView
            .RPC("AddAttackMessage", RpcTarget.All, string.Format("{0}自杀了", gameObject.name));
        FindObjectOfType<PhotonGame>().photonView.RPC("DeadAI", RpcTarget.All, transform.name);
    }

    public void RebornStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.position = FindObjectOfType<PhotonGame>().GroundRunwayPosition[index].position +
                                 new Vector3(0, 15, 0);
            transform.rotation = Quaternion.identity;
        }

        time = 10.0f;
        maxTime = 10;

        reborn = true;
    }

    void RebornEnd()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        reborn = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "FX")
        {
            Suicide();
        }
    }
}
