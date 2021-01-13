using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LittleHealthBar : MonoBehaviour
{
    [SerializeField] private Transform uiRoot;
    [SerializeField] private GameObject littleHealthBarPrefab;
    private readonly List<Transform> _forwardTargetList = new List<Transform>();
    private readonly List<GameObject> _littleHealthBarList = new List<GameObject>();
    private Transform _currentPlane = null;
    private bool _isInitialize = false;
    private const float MAXDistance = 10000.0f;

    // Update is called once per frame
    void Update()
    {
        if (!_isInitialize || !_currentPlane) return;

        GetForwardTargetsTransform();

        _littleHealthBarList.ForEach(delegate(GameObject littleHealthBar)
        {
            _forwardTargetList.ForEach(delegate(Transform target)
            {
                if (target.CompareTag("Plane") &&
                    target.GetComponent<PhotonView>().Controller.NickName == littleHealthBar.name &&
                    !Equals(target, _currentPlane))
                {
                    littleHealthBar.transform.Find("Health").GetComponent<Image>().fillAmount =
                        (float) ((int) target.GetComponent<PhotonView>().Controller.GetProperties("HP", 100) / 100.0);
                    littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount = Mathf.Lerp(
                        (float) ((int) target.GetComponent<PhotonView>().Controller.GetProperties("HP", 100) / 100.0),
                        littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount, 0.95f);

                    Vector3 rectPosition = _currentPlane.transform.Find("ShakeCamera").Find("MultipurposeCameraRig")
                        .Find("Pivot").Find("MainCamera").GetComponent<Camera>()
                        .WorldToScreenPoint(target.position);
                    littleHealthBar.GetComponent<Image>().rectTransform.position =
                        new Vector3(rectPosition.x, rectPosition.y + 10.0f, rectPosition.z);

                }
                else if (target.CompareTag("AI") &&
                         target.name == littleHealthBar.name &&
                         !Equals(target, _currentPlane))
                {

                    littleHealthBar.transform.Find("Health").GetComponent<Image>().fillAmount =
                        (float) (target.GetComponent<AIProperty>().HP / 100.0);
                    littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount = Mathf.Lerp(
                        (float) (target.GetComponent<AIProperty>().HP / 100.0),
                        littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount, 0.95f);

                    Vector3 rectPosition = _currentPlane.transform.Find("ShakeCamera").Find("MultipurposeCameraRig")
                        .Find("Pivot").Find("MainCamera").GetComponent<Camera>()
                        .WorldToScreenPoint(target.position);
                    littleHealthBar.GetComponent<Image>().rectTransform.position =
                        new Vector3(rectPosition.x, rectPosition.y + 10.0f, rectPosition.z);
                }
            });
        });
    }

    public void Initialize(Transform currentPlane)
    {
        _isInitialize = false;
        
        _littleHealthBarList.Clear();

        _currentPlane = currentPlane;

        PhotonNetwork.PlayerList.ToList().ForEach(delegate(Player target)
        {
            GameObject littleHealthBar = Instantiate(littleHealthBarPrefab, uiRoot);
            littleHealthBar.transform.Find("Name").GetComponent<Text>().text = target.NickName;
            littleHealthBar.transform.Find("Health").GetComponent<Image>().fillAmount =
                (float) ((int) target.GetProperties("HP", 100) / 100.0);
            littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount =
                (float) ((int) target.GetProperties("HP", 100) / 100.0);

            littleHealthBar.name = target.NickName;

            _littleHealthBarList.Add(littleHealthBar);
        });

        FindObjectOfType<PhotonGameAI>().AiPlaneList.ForEach(delegate(GameObject aiTarget)
        {
            GameObject littleHealthBar = Instantiate(littleHealthBarPrefab, uiRoot);
            littleHealthBar.transform.Find("Name").GetComponent<Text>().text = aiTarget.name;
            littleHealthBar.transform.Find("Health").GetComponent<Image>().fillAmount =
                (float) (aiTarget.GetComponent<AIProperty>().HP / 100.0);
            littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount =
                (float) (aiTarget.GetComponent<AIProperty>().HP / 100.0);

            littleHealthBar.name = aiTarget.name;
            littleHealthBar.SetActive(!aiTarget.GetComponent<AIProperty>().isDead);

            _littleHealthBarList.Add(littleHealthBar);
        });

        _isInitialize = true;
    }

    private void GetForwardTargetsTransform()
    {
        _forwardTargetList.Clear();
        
        List<Transform> missileTargets = new List<Transform>();

        GameObject.FindGameObjectsWithTag("Plane").ToList().ForEach(target => missileTargets.Add(target.transform));
        GameObject.FindGameObjectsWithTag("AI").ToList().ForEach(aiTarget => missileTargets.Add(aiTarget.transform));

        Vector3 thisPosition = _currentPlane.position;
        float[] distances = new float[missileTargets.Count];
        for (int i = 0; i < missileTargets.Count; i++)
        {
            Vector3 targetPosition = ((Transform) missileTargets[i]).position;
            Vector3 dir = targetPosition - thisPosition;
            float dot = Vector3.Dot(Vector3.forward, dir);
            if (dot > 0)
                distances[i] = Vector3.Distance(thisPosition, targetPosition);
            else
                distances[i] = 0;
        }

        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] != 0 && distances[i] < MAXDistance)
                _forwardTargetList.Add(missileTargets[i] as Transform);
        }
    }

    [PunRPC]
    public void LittleHeathBarReload(bool changePlane, Transform currentPlane)
    {
        _littleHealthBarList.ForEach(Destroy);

        Initialize(changePlane ? currentPlane : _currentPlane);
    }
}
