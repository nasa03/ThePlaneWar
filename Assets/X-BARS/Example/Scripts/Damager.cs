using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Damager : MonoBehaviour {

	public int Power = 10;
	public AudioClip Shot;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>(); 
    }

	void Update() 
	{
		if (Input.GetButtonDown("Fire1")) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Enemy"))
			{
                source.clip = Shot;
                source.Play();

				//Sending Helth decrease message
				hit.collider.SendMessage("GetDamage", Power);
			}
		}
	}
}
