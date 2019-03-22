using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosePlane : MonoBehaviour {
    [SerializeField] GameObject[] planePrefabs;
    GameObject totalPlaneObject;
    [SerializeField] [HideInInspector] int totalPlaneInt = 0;

    // Use this for initialization
    void Start()
    {
        totalPlaneObject = Instantiate(planePrefabs[totalPlaneInt], new Vector3(0, 5, 0), Quaternion.identity);
        totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);
    }

    public int TotalPlaneInt
    {
        get
        {
            return totalPlaneInt;
        }
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
        totalPlaneObject = Instantiate(planePrefabs[totalPlaneInt], new Vector3(0, 5, 0), Quaternion.identity);
        totalPlaneObject.transform.localScale = new Vector3(5, 5, 5);
    }
}
