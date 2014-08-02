using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	public float breakThreshold = 2f;
	public float health = 20f;

	void OnCollisionEnter2D(Collision2D col){
		if(	(col.relativeVelocity.y < -5f) && 
			(col.transform.position.y < transform.position.y) && // Pretty much just only deal damage if the collision happened 
			(col.collider.CompareTag("Enemy") || col.collider.CompareTag("Player"))){ // underneath the crate

			col.collider.SendMessage("TakeDamage", col.relativeVelocity.magnitude, SendMessageOptions.DontRequireReceiver);
		}

		if(col.relativeVelocity.magnitude > breakThreshold) TakeDamage(col.relativeVelocity.magnitude - breakThreshold);
	}

	void TakeDamage(float dmg){
		health -= dmg;

		Game.main.CreateBlamo(transform.position, dmg);
		if(health <= 0f) Break();
	}

	void Break(){
		// Spawn proxy for particle effect
		Destroy(gameObject);
	}
}
