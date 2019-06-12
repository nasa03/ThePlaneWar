using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RadarObject {
	
	public Image icon { get; set; }
	public GameObject owner { get; set; }
	
}


public class BinnacleScript : MonoBehaviour {
	
	public static BinnacleScript Instance;
	[Tooltip("Select your player character. The radar will use this for it's position.")]
	public Transform player;
	[Tooltip("Select your main playerCamera. The radar will use this to determine which direction your player if facing.")]
	public Transform playerCamera;
	
	[Tooltip("Select the camera you want to use for the MiniMap.")]
	public Camera miniMapCamera;
	
	[Tooltip("Select an image or rect prefab to use as a default marker. This can be overridden on each tracked object.")]
	public Image defaulMarkerImage;
	public static Image fallbackMarkerImage;
	
	[Tooltip("The radius of your radar in unity units. This will also be modified by the Radar Mask Scale.")]
	public float radarRadius = 20.0f;
	//[Tooltip("This allows you to scale down the mask for the markers on the radar. This is useful if you replace the radar graphic.")]
	//public float radarMaskScale = 0.86f;
	
	[Tooltip("If Marker Compassing is selected the the markers will rotate around the radar like a compass as the player looks around. Turn this off if you want the radar marks to to rotate e.g. a FPS with a players frustum which rotates instead. This will also control if the Mini Map will compass or not.")]
	public bool markerCompassing = true;
	
	[Tooltip("You can use this to modify the direction of north. This is in degrees.")]
	public float northModifier = 0.0f;
	
	[Tooltip("Add any objects here which you want to point north.")]
	public List<GameObject> compassingObjects = new List<GameObject>();
	[Tooltip("Add any objects here which you wish to point in the same direction as the player character.")]
	public List<GameObject> reverseCompassingObjects = new List<GameObject>();
	
	private static List<RadarObject> radarMarkers = new List<RadarObject>();
	private Transform radarLayer;
	
	void Start (){
		
		if (defaulMarkerImage) {
			fallbackMarkerImage = defaulMarkerImage;
		} else {
			Debug.LogError("RADAR: You need to an image to the \"Default Marker Image\" public variable, it can be an image file or a rect prefab.\n You must do this or any tracked object without a custom marker will not display");
		}
		
		if (!player) {
			Debug.LogError("RADAR: You must assign a player!");
		}
		
		if (!playerCamera) {
			Debug.LogError("RADAR: You must assign a playerCamera!");
		}
		
		radarLayer = transform.Find("__RadarLayer");
		
	}
	
	void Update (){
		
		DrawRadarMarkers ();
		MiniMapPositioning ();
		
		foreach (GameObject compassingObject in compassingObjects) {
			Compassing (compassingObject, false);
		}
		
		foreach (GameObject reverseCompassingObject in reverseCompassingObjects) {
			Compassing (reverseCompassingObject, true);
		}
		
	}
	
	
	public static void RegisterRadarObject(GameObject radarMarker_owner, Image radarMarker_image){
		
		Image radarMarker_icon;
		
		if (!fallbackMarkerImage){ 
			//return;
		}
		
		if (radarMarker_image != null) {
			radarMarker_icon = Instantiate(radarMarker_image);
		} else {
			radarMarker_icon = Instantiate(fallbackMarkerImage);
		}
		
		radarMarkers.Add (new RadarObject (){owner = radarMarker_owner, icon = radarMarker_icon});
		
	}
	
	
	public static void RemoveRadarObject(GameObject radarMarker_owner){
		
		List<RadarObject> newList = new List<RadarObject>();
		
		for (int i = 0; i < radarMarkers.Count; i++){
			if ( radarMarkers[i].owner == radarMarker_owner){
				Destroy(radarMarkers[i].icon);
				continue;
			} else {
				newList.Add(radarMarkers[i]);
			}
		}
		
		radarMarkers.RemoveRange(0, radarMarkers.Count);
		radarMarkers.AddRange(newList);
		
	}
	
	void DrawRadarMarkers(){
		
		foreach(RadarObject radarObject in radarMarkers){
			
			//Create parent and child RectTransforms
			RectTransform thisTransformRectTransform = this.transform as RectTransform;
			
			//Positioning
			Vector3 radarPos3D = (radarObject.owner.transform.position - player.position);
			float distanceToObject = Vector2.Distance( new Vector2(player.position.x, player.position.z), new Vector2(radarObject.owner.transform.position.x, radarObject.owner.transform.position.z)) / radarRadius;
			float deltay = Mathf.Atan2(radarPos3D.x, radarPos3D.z) * Mathf.Rad2Deg - 270;
			
			//Optional Marker Compassing
			if (markerCompassing){
				deltay = deltay - player.eulerAngles.y + northModifier;
			} else {
				deltay = deltay + northModifier;
			}
			
			//More Positioning
			radarPos3D.x = (distanceToObject * (Mathf.Cos(deltay * Mathf.Deg2Rad) * -1) * (thisTransformRectTransform.sizeDelta.x)) / 2;
			radarPos3D.z = (distanceToObject * Mathf.Sin(deltay * Mathf.Deg2Rad) * (thisTransformRectTransform.sizeDelta.y)) / 2;
			
			//Set Parent
			radarObject.icon.transform.SetParent(radarLayer.transform);
			
			//Apply Positioning
			radarObject.icon.transform.position = new Vector3(radarPos3D.x, radarPos3D.z, 0) + radarLayer.transform.position;
			
		}
	}
	
	void Compassing( GameObject compassingObject, bool reversed ){
		if (reversed) {
			compassingObject.transform.rotation = Quaternion.AngleAxis ((player.eulerAngles.y - northModifier), Vector3.back);
		} else {
			compassingObject.transform.rotation = Quaternion.AngleAxis ((player.eulerAngles.y - northModifier), Vector3.forward);
		}
		
	}
	
	void MiniMapPositioning(){
		if (player && miniMapCamera) {
			//miniMapCamera.transform.SetParent(player);
			miniMapCamera.transform.position = new Vector3 (player.position.x, 1200, player.position.z);
			miniMapCamera.orthographicSize = radarRadius;
			
			if ( markerCompassing == true ){
				miniMapCamera.transform.rotation = Quaternion.Euler(90, ( player.eulerAngles.y - northModifier), 0);
			} else if ( markerCompassing == false ) {
				miniMapCamera.transform.rotation = Quaternion.Euler(90, -northModifier, 0);
			}
		}
	}
	
}
