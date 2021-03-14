using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISuicide
{
    IEnumerator Suicide();

    void OnCollisionEnter(Collision collision);
    
}