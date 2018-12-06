using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : SingletonManager<GlobalManager> {
    [SerializeField] GameObject[] playerPrefabs;

    public GameObject PlayerPrefab
    {
        get
        {
            return playerPrefabs[ChoosePlane.Instance.TotalPlaneInt];
        }
    }
}
