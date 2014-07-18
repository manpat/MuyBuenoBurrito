using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {
	public float breakThreshold = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.relativeVelocity.magnitude > breakThreshold) Break();
	}

	void Break(){
		renderer.material.color = Color.red;
	}
}
