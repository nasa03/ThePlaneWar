using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExplodingMissile : MonoBehaviourPun
{
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float speed = 800;
    [SerializeField] private float explosionTimer = 6;
    private Rigidbody _thisRigidbody;
    private Collider _thisCollider;
    private Transform _missileTarget;
    private float _timer;

    // Start is called before the first frame update
    private void Start()
    {
        Instantiate(muzzlePrefab);
        _missileTarget = GetNearTargetTransform();
    }

    // Update is called once per frame
    private void Update()
    {
        _timer += Time.deltaTime;
        if (!(_timer >= explosionTimer)) return;

        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        PhotonNetwork.Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.LookAt(_missileTarget != null ? _missileTarget : transform);

        _thisRigidbody.AddForce(transform.forward * speed);

        if (_timer >= 0.05f)
            transform.rotation = Quaternion.LookRotation(_thisRigidbody.velocity);
    }

    private void Awake()
    {
        _thisRigidbody = GetComponent<Rigidbody>();
        _thisCollider = GetComponent<Collider>();
    }

    private Transform GetNearTargetTransform()
    {
        ArrayList missileTargets = new ArrayList();
        
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject target in targets)
        {
            if (!target.GetComponent<PhotonView>().IsMine)
                missileTargets.Add(target.transform);
        }

        GameObject[] aiTargets = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject target in aiTargets)
        {
            missileTargets.Add(target.transform);
        }

        Vector3 thisPosition = transform.position;
        float[] distances = new float[missileTargets.Count];
        for (int i = 0; i < missileTargets.Count; i++)
        {
            Vector3 targetPosition = (missileTargets[i] as Transform).position;
            Vector3 dir = targetPosition - thisPosition;
            float dot = Vector3.Dot(Vector3.forward, dir);
            if (dot > 0)
                distances[i] = Vector3.Distance(thisPosition, targetPosition);
            else
                distances[i] = 0;
        }

        float minDistance = 0;
        Transform minTarget = null;
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] != 0 && (minDistance == 0 || distances[i] < minDistance))
            {
                minDistance = distances[i];
                minTarget = missileTargets[i] as Transform;
            }
        }

        return minTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FX") || collision.gameObject.CompareTag("Plane") ||
            collision.gameObject.CompareTag("AI")) return;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        Vector3 pos = contact.point;
        Instantiate(explosionPrefab, pos, rot);
        _thisCollider.enabled = false;
        _thisRigidbody.velocity = Vector3.zero;

        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        AIBullet aiBullet = GetComponent<AIBullet>();

        if (aiBullet == null)
        {
            switch (other.gameObject.tag)
            {
                case "Plane" when !other.GetComponent<PhotonView>().IsMine:
                    FindObjectOfType<PhotonGame>().LocalPlane.GetComponent<PlaneAttack>()
                        .Attack(other.GetComponent<PhotonView>().Controller, other.transform);

                    PhotonNetwork.Destroy(gameObject);
                    break;
                case "AI":
                    FindObjectOfType<PhotonGame>().LocalPlane.GetComponent<PlaneAttack>()
                        .AttackAI(other.transform);

                    PhotonNetwork.Destroy(gameObject);
                    break;
            }
        }
        else
        {
            switch (other.gameObject.tag)
            {
                case "Plane" when !other.GetComponent<PhotonView>().IsMine:
                    aiBullet.aiTarget.GetComponent<AIAttack>()
                        .Attack(other.GetComponent<PhotonView>().Controller, other.transform);

                    PhotonNetwork.Destroy(gameObject);
                    break;
                case "AI":
                    aiBullet.aiTarget.GetComponent<AIAttack>().AttackAI(other.transform);

                    PhotonNetwork.Destroy(gameObject);
                    break;
            }
        }
    }
}