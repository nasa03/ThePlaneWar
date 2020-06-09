using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {
    public static readonly string path = string.Format("{0}/name", Application.persistentDataPath);

    public static readonly Vector3[] planePositions =
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
        normal, exitGame, gameOver, disconnected
    };

    public static int totalPlaneInt = 0;
    public static int totalPlayerInt = 0;
    public static int totalFireInt = 0;
    public static int totalMissileInt = 0;
    public static bool isOffline = false;
    public static ReturnState returnState = ReturnState.normal;
}
