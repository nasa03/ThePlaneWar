using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePlane : MonoBehaviour {
    [SerializeField] GameObject[] planePrefabs;
    GameObject totalPlaneObject;
    int totalPlaneInt = 0;
    public int TotalPlaneInt { get { return totalPlaneInt; } }

    // Use this for initialization
    void Start()
    {
        totalPlaneObject = Instantiate(planePrefabs[totalPlaneInt], new Vector3(0, 5, -15), Quaternion.Euler(0, 180, 0));
        totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);

        Show(false);
    }

    public void Show(bool isShow)
    {
        totalPlaneObject.SetActive(isShow);
    }

    public void NextPlane()
    {
        totalPlaneInt++;
        if (totalPlaneInt > planePrefabs.Length - 1)
            totalPlaneInt = 0;

        ChangePlane();
    }

    public void LastPlane()
    {
        totalPlaneInt--;
        if (totalPlaneInt < 0)
            totalPlaneInt = planePrefabs.Length - 1;

        ChangePlane();
    }

    void ChangePlane()
    {
        Destroy(totalPlaneObject);
        totalPlaneObject = Instantiate(planePrefabs[totalPlaneInt], new Vector3(0, 5, -15), Quaternion.Euler(0, 180, 0));
        totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);
    }
}
