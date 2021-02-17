using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public static class Global {
    public static readonly string Path = $"{Application.persistentDataPath}/name";
    public static readonly string JsonPath = $"{Application.persistentDataPath}/setting.json";

    public static readonly Vector3[] PlanePositions =
    {
        new Vector3 (0,5,-15),
        new Vector3 (-12,5,-8),
        new Vector3 (12,5,-8),
        new Vector3 (-24,5,5),
        new Vector3 (24,5,5),
        new Vector3 (0,5,5)
    };

    public enum ReturnState
    {
        Normal, ExitGame, GameOver, Disconnected
    };
    
    public struct AIPlaneScores
    {
        public string name;
        public int kill;
        public int death;
    }

    public static int totalPlaneInt = 0;
    public static int totalPlayerInt = 0;
    public static int totalFireInt = 0;
    public static int totalMissileInt = 0;
    public static bool bGyroscopeEnabled = false;
    public static float gyroscopeMultiple = 1.0f;
    public static bool isOffline = false;
    public static ReturnState returnState = ReturnState.Normal;
    
    public static readonly ArrayList aiPlaneScores = new ArrayList();
    
    public static Transform GetNearTargetTransform(Transform current)
    {
        List<Transform> missileTargets = new List<Transform>();

        GameObject.FindGameObjectsWithTag("Plane").ToList().ForEach(delegate(GameObject target)
        {
            if (!target.GetComponent<PhotonView>().IsMine) 
                missileTargets.Add(target.transform);
        });

        GameObject.FindGameObjectsWithTag("AI").ToList().ForEach(aiTarget => missileTargets.Add(aiTarget.transform));

        Vector3 thisPosition = current.position;
        float[] distances = new float[missileTargets.Count];
        for (int i = 0; i < missileTargets.Count; i++)
        {
            Vector3 targetPosition = (missileTargets[i]).position;
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
                minTarget = missileTargets[i];
            }
        }

        return minTarget;
    }
}
