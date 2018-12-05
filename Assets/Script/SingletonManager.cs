using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : Component {
    static T instance;

    public static T GetInstance()
    {
        return FindObjectOfType<T>() as T;
    }
    
}