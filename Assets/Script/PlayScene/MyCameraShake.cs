using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraShake : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log(FindObjectOfType<CameraShake>());
        FindObjectOfType<CameraShake>().ShakeCamera();
    }
}
