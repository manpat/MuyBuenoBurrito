using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	public float breakThreshold = 2f;

	void OnCollisionEnter2D(Collision2D col){
		if(col.relativeVelocity.magnitude > breakThreshold) Break();
	}

	void Break(){
		// Spawn proxy for particle effect
		Destroy(gameObject);
	}
}
