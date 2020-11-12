using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;
public class Bullet : NetworkBehaviour {

	void OnCollisionEnter(Collision collison){

		GameObject hit = collison.gameObject;
		Health health = hit.GetComponent<Health> ();
		if (health != null) 
		{
			health.TakeDamage (10);
		}
		Destroy (gameObject);
	}
}

