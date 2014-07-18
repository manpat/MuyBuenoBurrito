using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float playerBottom = Game.main.player.transform.position.y - Game.main.player.transform.localScale.y/2f;
		float platformTop = transform.position.y + transform.localScale.y/2f;

		Debug.DrawLine(new Vector3(-1000, playerBottom), new Vector3(1000, playerBottom));
		Debug.DrawLine(new Vector3(transform.position.x-collider2D.bounds.extents.x, platformTop), new Vector3(transform.position.x+collider2D.bounds.extents.x, platformTop));

		if(playerBottom >= platformTop){
			// renderer.material.color = Color.green;
			collider2D.enabled = true;
		}else{
			// renderer.material.color = Color.red;
			collider2D.enabled = false;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		
	}
}
