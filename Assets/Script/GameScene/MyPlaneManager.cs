using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyPlaneManager : NetworkBehaviour {
    [SerializeField] GameObject cameraPrefab;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        cameraPrefab.SetActive(true);
    }
}
