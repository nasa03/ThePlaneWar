using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {

	public void ReloadLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
