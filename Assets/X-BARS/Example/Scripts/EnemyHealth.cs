using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int Health;
    private Animation anim;

	// Update is called once per frame
    void Start()
    {
        anim = GetComponent<Animation>();
    }

	void Update () {
        if (Health <= 0)
        {
            anim.Play("Death");
        }
        else
            anim.Play("Idle");
	}


	public void GetDamage(int damage)
	{
		Health -= damage;
	}
}
