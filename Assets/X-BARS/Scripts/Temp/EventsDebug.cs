using UnityEngine;
using System.Collections;

public class EventsDebug : MonoBehaviour {

	public void OnDecrease(GameObject sourceObject)
    {
        Debug.Log(sourceObject.name + " was damaged");
    }

    public void OnDeath(GameObject sourceObject)
    {
        Debug.Log(sourceObject.name + " is dead");
    }

    public void OnIncrease(GameObject sourceObject)
    {
        Debug.Log(sourceObject.name +"'s health was increased");
    }

    public void OnChange(GameObject sourceObject)
    {
        Debug.Log(sourceObject.name + " was changed");
    }

    public void OnFull(GameObject sourceObject)
    {
        Debug.Log(sourceObject.name +"'s  was restored");
    }
}
