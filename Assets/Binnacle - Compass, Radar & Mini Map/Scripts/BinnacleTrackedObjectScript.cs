using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BinnacleTrackedObjectScript : MonoBehaviour {
	
	public Image radarMarkerImage;
	
	void Start () {
		BinnacleScript.RegisterRadarObject (this.gameObject, radarMarkerImage);
	}
	
	void OnDestroy (){
		BinnacleScript.RemoveRadarObject (this.gameObject);
	}
	
}
