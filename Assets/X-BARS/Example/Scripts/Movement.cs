using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Movement : MonoBehaviour {

	public Slider moveSlider, rotateSlider;
	public Transform pivot;
	private Vector3 newPos;
	private Vector3 newRot;

	void Start()
	{
		newPos = transform.localPosition;
		newRot = pivot.eulerAngles;
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, 0.1F);
		pivot.eulerAngles = Vector3.Lerp(pivot.eulerAngles, newRot, 0.1F);
	}


	public void moveCam()
	{
		newPos.z = -moveSlider.value;
	}

	public void rotateCam()
	{
		newRot.y = rotateSlider.value;
	}
}
