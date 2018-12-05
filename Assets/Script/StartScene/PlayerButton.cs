using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButton : MonoBehaviour {
    [SerializeField] UIInput input;

    public void OnClick()
    {
        GlobalManager.GetInstance().playerIndex = int.Parse(input.value);
    }
	
}
