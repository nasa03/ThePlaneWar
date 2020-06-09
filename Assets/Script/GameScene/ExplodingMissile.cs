using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExplodingMissile : MonoBehaviour
{
    [SerializeField] GameObject muzzlePrefab;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float speed = 800;
    [SerializeField] float explosionTimer = 6;
    Rigidbody thisRigidbody;
    Collider thisCollider;
    Transform missileTarget;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(muzzlePrefab);
        missileTarget = GetNearTargetTransform();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= explosionTimer)
        {
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (missileTarget != null)
            transform.LookAt(missileTarget);
        else
            transform.LookAt(transform);

        thisRigidbody.AddForce(transform.forward * speed);

        if (timer >= 0.05f)
            transform.rotation = Quaternion.LookRotation(thisRigidbody.velocity);

    }

    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
    }

    Transform GetNearTargetTransform()
    {
        ArrayList missileTargets = new ArrayList();
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");
        
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].GetComponent<PhotonView>().IsMine)
                missileTargets.Add(targets[i].transform);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "FX" && collision.gameObject.tag != "Plane")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(explosionPrefab, pos, rot);
            thisCollider.enabled = false;
            thisRigidbody.velocity = Vector3.zero;

            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        if (other.gameObject.tag == "Plane" && !other.GetComponent<PhotonView>().IsMine)
        {
            FindObjectOfType<PlaneAttack>().Attack(other.gameObject.GetComponent<PhotonView>().Controller);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
