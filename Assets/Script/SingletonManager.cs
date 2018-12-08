using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : Component {
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>() as T;

            return instance;
        }
    }
}