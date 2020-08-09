using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIAttack : MonoBehaviour
{
    PhotonGame photonGame;
    PhotonGameAI photonGameAi;
    PhotonView gameView;
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
        photonGame = FindObjectOfType<PhotonGame>();
        photonGameAi = FindObjectOfType<PhotonGameAI>();
        gameView = FindObjectOfType<PhotonGame>().GetComponent<PhotonView>();
        aiProperty = GetComponent<AIProperty>();
    }

    public void Attack(Player player, Transform target)
    {
        int randomAttack = Random.Range(5, 15);

        int totalHP = (int) CustomProperties.GetProperties(player, "HP", 100);
        bool invincible = (bool) CustomProperties.GetProperties(player, "invincible", false);

        if (totalHP <= 0 || invincible)
            return;

        totalHP -= randomAttack;
        CustomProperties.SetProperties(player, "HP", totalHP);

        gameView.RPC("PlayAudio", player, 2);

        if (totalHP <= 0)
        {
            aiProperty.Kill++;

            GetComponent<AudioSource>().Play();
            gameView.RPC("AddAttackMessage", RpcTarget.All,
                string.Format("{0}击杀了{1}", aiProperty.Name, player.NickName));
            gameView.RPC("Dead", player);
        }
    }

    public void AttackAI(Transform target)
    {
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
            gameView.RPC("AddAttackMessage", RpcTarget.All,
                string.Format("{0}击杀了{1}", aiProperty.Name, targetProperty.Name));
            target.GetComponent<AIAttack>().Dead();
        }
    }

    void Suicide()
    {
        gameView.RPC("AddAttackMessage", RpcTarget.All, string.Format("{0}自杀了", aiProperty.Name));
        Dead();
    }
    
    public void Dead()
    {
        GameObject explosion = PhotonNetwork.Instantiate(photonGame.ExplosionParticleSystem.name, transform.position, Quaternion.identity);
        explosion.GetComponent<ParticleSystem>().Play();
        GetComponent<AudioPlayer>().PlayAudio(3);
        
        GetComponent<MeshRenderer>().enabled = false;
        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (var items in particleSystems)
        {
            items.Stop();
        }

        RebornStart();

        aiProperty.Death++;

        aiProperty.HP = 100;
    }
    
    void RebornStart()
    {
        gameObject.SetActive(false);

        time = 10.0f;
        maxTime = 10;

        reborn = true;
    }
    
    void RebornEnd()
    {
        gameObject.SetActive(true);
        transform.position = photonGame.GroundRunwayPosition[index].position + new Vector3(0, 15, 0);
        transform.rotation = Quaternion.identity;

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
