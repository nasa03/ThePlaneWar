using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LittleHealthBar : MonoBehaviour
{
    [SerializeField] private Transform uiRoot;
    [SerializeField] private GameObject littleHealthBarPrefab;
    private readonly ArrayList _forwardTargetList = new ArrayList();
    private readonly ArrayList _littleHealthBarList = new ArrayList();
    private Transform _currentPlane = null;
    private bool _isInitialize = false;
    private const float MAXDistance = 10000.0f;

    // Update is called once per frame
    void Update()
    {
        if (!_isInitialize || !_currentPlane) return;
        
        GetForwardTargetsTransform();

        foreach (GameObject littleHealthBar in _littleHealthBarList)
        {
            foreach (Transform target in _forwardTargetList)
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
            }
        }
    }

    public void Initialize(Transform currentPlane)
    {
        _isInitialize = false;
        
        _littleHealthBarList.Clear();

        _currentPlane = currentPlane;

        foreach (Player target in PhotonNetwork.PlayerList)
        {
            GameObject littleHealthBar = Instantiate(littleHealthBarPrefab, uiRoot);
            littleHealthBar.transform.Find("Name").GetComponent<Text>().text = target.NickName;
            littleHealthBar.transform.Find("Health").GetComponent<Image>().fillAmount =
                (float) ((int) target.GetProperties("HP", 100) / 100.0);
            littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount =
                (float) ((int) target.GetProperties("HP", 100) / 100.0);

            littleHealthBar.name = target.NickName;

            _littleHealthBarList.Add(littleHealthBar);
        }
        
        foreach (GameObject aiTarget in FindObjectOfType<PhotonGameAI>().AiPlaneList)
        { 
            GameObject littleHealthBar = Instantiate(littleHealthBarPrefab, uiRoot);
            littleHealthBar.transform.Find("Name").GetComponent<Text>().text = aiTarget.name;
            littleHealthBar.transform.Find("Health").GetComponent<Image>().fillAmount =
                (float) (aiTarget.GetComponent<AIProperty>().HP / 100.0);
            littleHealthBar.transform.Find("HealthLerp").GetComponent<Image>().fillAmount =
                (float) (aiTarget.GetComponent<AIProperty>().HP / 100.0);

            littleHealthBar.name = aiTarget.name;

            _littleHealthBarList.Add(littleHealthBar);
        }

        _isInitialize = true;
    }

    private void GetForwardTargetsTransform()
    {
        _forwardTargetList.Clear();
        
        ArrayList missileTargets = new ArrayList();

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Plane");
        foreach (GameObject target in targets)
        {
            missileTargets.Add(target.transform);
        }

        GameObject[] aiTargets = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject target in aiTargets)
        {
            missileTargets.Add(target.transform);
        }

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
            {
                _forwardTargetList.Add(missileTargets[i] as Transform);
            }
        }
    }

    [PunRPC]
    public void LittleHeathBarReload(bool changePlane, Transform currentPlane)
    {
        foreach (GameObject littleHealthBar in _littleHealthBarList)
        {
            Destroy(littleHealthBar);
        }

        Initialize(changePlane ? currentPlane : _currentPlane);
    }
}
