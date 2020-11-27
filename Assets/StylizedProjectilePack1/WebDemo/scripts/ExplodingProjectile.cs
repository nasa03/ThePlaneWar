using UnityEngine;
using System.Collections;
using Photon.Pun;

/* THIS CODE IS JUST FOR PREVIEW AND TESTING */
// Feel free to use any code and picking on it, I cannot guaratnee it will fit into your project
public class ExplodingProjectile : MonoBehaviourPun
{
    public GameObject impactPrefab;
    public GameObject explosionPrefab;
    public float thrust;

    public Rigidbody thisRigidbody;

    public GameObject particleKillGroup;
    private Collider thisCollider;

    public bool LookRotation = true;
    public bool Missile = false;
    public Transform missileTarget;
    public float projectileSpeed;
    public float projectileSpeedMultiplier;

    public bool ignorePrevRotation = false;

    public bool explodeOnTimer = false;
    public float explosionTimer;
    float timer;

    private Vector3 previousPosition;

    // Use this for initialization
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        if (Missile)
        {
            missileTarget = GetNearTargetTransform();
        }
        thisCollider = GetComponent<Collider>();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        /*     if(Input.GetButtonUp("Fire2"))
             {
                 Explode();
             }*/
        timer += Time.deltaTime;
        if (timer >= explosionTimer && explodeOnTimer == true)
        {
            Explode();
        }

    }
    
    private Transform GetNearTargetTransform()
    {
        ArrayList projectileTargets = new ArrayList();
        
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject target in targets)
        {
            if (!target.GetComponent<PhotonView>().IsMine)
                projectileTargets.Add(target.transform);
        }

        GameObject[] aiTargets = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject target in aiTargets)
        {
            projectileTargets.Add(target.transform);
        }

        Vector3 thisPosition = transform.position;
        float[] distances = new float[projectileTargets.Count];
        for (int i = 0; i < projectileTargets.Count; i++)
        {
            Vector3 targetPosition = (projectileTargets[i] as Transform).position;
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
                minTarget = projectileTargets[i] as Transform;
            }
        }

        return minTarget;
    }

    void FixedUpdate()
    {
        if (Missile)
        {
            projectileSpeed += projectileSpeed * projectileSpeedMultiplier;
            //   transform.position = Vector3.MoveTowards(transform.position, missileTarget.transform.position, 0);

            transform.LookAt(missileTarget);

            thisRigidbody.AddForce(transform.forward * projectileSpeed);
        }

        if (LookRotation && timer >= 0.05f)
        {
            transform.rotation = Quaternion.LookRotation(thisRigidbody.velocity);
        }

        //CheckCollision(previousPosition);

        previousPosition = transform.position;
    }

    void CheckCollision(Vector3 prevPos)
    {

        RaycastHit hit;
        Vector3 direction = transform.position - prevPos;
        Ray ray = new Ray(prevPos, direction);
        float dist = Vector3.Distance(transform.position, prevPos);
        if (Physics.Raycast(ray, out hit, dist))
        {
            transform.position = hit.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Vector3 pos = hit.point;
            Instantiate(impactPrefab, pos, rot);
            if (!explodeOnTimer && Missile == false)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else if (Missile == true)
            {
                thisCollider.enabled = false;
                particleKillGroup.SetActive(false);
                thisRigidbody.velocity = Vector3.zero;
                StartCoroutine(DestroyAfterTime(5));
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FX") || collision.gameObject.CompareTag("Plane") ||
            collision.gameObject.CompareTag("AI")) return;
        
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        if (ignorePrevRotation)
        {
            rot = Quaternion.Euler(0, 0, 0);
        }

        Vector3 pos = contact.point;
        Instantiate(impactPrefab, pos, rot);
        if (!explodeOnTimer && Missile == false)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else if (Missile == true)
        {

            thisCollider.enabled = false;
            particleKillGroup.SetActive(false);
            thisRigidbody.velocity = Vector3.zero;

            StartCoroutine(DestroyAfterTime(5));

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        AIBullet aiBullet = GetComponent<AIBullet>();

        if (aiBullet == null)
        {
            if (other.gameObject.CompareTag("Plane") && !other.GetComponent<PhotonView>().IsMine)
            {
                FindObjectOfType<PhotonGame>().LocalPlane.GetComponent<PlaneAttack>()
                    .Attack(other.GetComponent<PhotonView>().Controller, other.transform);

                PhotonNetwork.Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("AI"))
            {
                FindObjectOfType<PhotonGame>().LocalPlane.GetComponent<PlaneAttack>()
                    .AttackAI(other.transform);

                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Plane") && !other.GetComponent<PhotonView>().IsMine)
            {
                aiBullet.aiTarget.GetComponent<AIAttack>()
                    .Attack(other.GetComponent<PhotonView>().Controller, other.transform);

                PhotonNetwork.Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("AI"))
            {
                aiBullet.aiTarget.GetComponent<AIAttack>().AttackAI(other.transform);

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator DestroyAfterTime(float t)
    {
        yield return new WaitForSeconds(t);
        PhotonNetwork.Destroy(gameObject);
    }

}