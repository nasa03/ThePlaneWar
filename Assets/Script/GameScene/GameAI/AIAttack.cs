using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIAttack : MonoBehaviour
{
    private AIProperty _aiProperty;
    private bool _reborn = false;

    private float _time = 0.0f;
    private int _maxTime = 0;
    private int _index;

    public int Index
    {
        set => _index = value;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            if (_reborn)
            {
                RebornEnd();
            }
        }
    }

    private void Awake()
    {
        _aiProperty = GetComponent<AIProperty>();
    }

    public void Attack(Player player, Transform target)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int randomAttack = Random.Range(5, 15);

        int totalHP = (int) player.GetProperties("HP", 100);
        bool invincible = (bool) player.GetProperties("invincible", false);

        if (totalHP <= 0 || invincible)
            return;

        totalHP -= randomAttack;
        player.SetProperties("HP", totalHP);

        FindObjectOfType<PhotonGame>().photonView.RPC("PlayAudio", player, 2);

        if (totalHP <= 0)
        {
            _aiProperty.Kill++;

            GetComponent<AudioSource>().Play();
            FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
                $"{gameObject.name}击杀了{player.NickName}");
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
            _aiProperty.Kill++;

            GetComponent<AudioSource>().Play();
            FindObjectOfType<PhotonGame>().photonView.RPC("AddAttackMessage", RpcTarget.All,
                $"{gameObject.name}击杀了{target.name}");
            FindObjectOfType<PhotonGame>().photonView.RPC("DeadAI", RpcTarget.All, target.name);
        }
    }

    private void Suicide()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        FindObjectOfType<PhotonGame>().photonView
            .RPC("AddAttackMessage", RpcTarget.All, $"{gameObject.name}自杀了");
        FindObjectOfType<PhotonGame>().photonView.RPC("DeadAI", RpcTarget.All, transform.name);
    }

    public void RebornStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.position = FindObjectOfType<PhotonGame>().GroundRunwayPosition[_index].position +
                                 new Vector3(0, 15, 0);
            transform.rotation = Quaternion.identity;
        }
        
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var items in particleSystems)
        {
            items.Stop();
        }

        _time = 10.0f;
        _maxTime = 10;

        _reborn = true;
    }

    private void RebornEnd()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;

        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var items in particleSystems)
        {
            items.Play();
        }

        _reborn = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FX"))
        {
            Suicide();
        }
    }
}
