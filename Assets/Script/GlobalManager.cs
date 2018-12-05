using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : SingletonManager<GlobalManager> {
    [SerializeField] GameObject[] playerPrefabs;
    [HideInInspector] public int playerIndex = 0;

    public GameObject GetPlayerPrefab()
    {
        Debug.Log(playerIndex);
        return playerPrefabs[playerIndex];
    }
}
