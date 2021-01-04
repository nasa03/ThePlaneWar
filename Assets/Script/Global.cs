using System.Collections;
using System.Collections.Generic;
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
}
