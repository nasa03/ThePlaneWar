using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject impactPrefab;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float speed;
    [SerializeField] float explosionTimer;
    Rigidbody thisRigidbody;
    Collider thisCollider;
    ArrayList missileTarget;
    float timer;

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
        int target = GetNearTargetTransform();
        transform.LookAt(missileTarget[target] as Transform);
        thisRigidbody.AddForce(transform.forward);

        if (timer >= 0.05f)
            transform.rotation = Quaternion.LookRotation(thisRigidbody.velocity);

    }


    private void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
    }

    int GetNearTargetTransform()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");
        missileTarget = new ArrayList();
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].GetComponent<PhotonView>().IsMine)
                missileTarget.Add(targets[i].transform);
        }

        Vector3 thisPosition = transform.position;
        float[] distances = new float[missileTarget.Count];
        for (int i = 0; i < missileTarget.Count; i++)
        {
            Vector3 targetposition = (missileTarget[i] as Transform).position;
            Vector3 dir = targetposition - thisPosition;
            float dot = Vector3.Dot(Vector3.forward, dir);
            if (dot > 0)
                distances[i] = Vector3.Distance(thisPosition, targetposition);
            else
                distances[i] = 0;
        }
        float minDistance = 0;
        int minTarget = 0;
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] != 0 && (minDistance == 0 || distances[i] < minDistance))
            {
                minDistance = distances[i];
                minTarget = i;
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
        if (other.gameObject.tag == "Plane" && !other.gameObject.GetComponent<PhotonView>().IsMine)
            FindObjectOfType<PlaneController>().Attack(other);
    }
}
