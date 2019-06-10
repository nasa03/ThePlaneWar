using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (new Vector3(0,0,0), Vector3.up, 20 * Time.deltaTime);
	}
}
