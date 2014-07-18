using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate () {
		float playerBottom = Game.main.player.transform.position.y - Game.main.player.transform.localScale.y/2f;
		float platformTop = transform.position.y + transform.localScale.y/2f;

		Debug.DrawLine(new Vector3(-1000, playerBottom), new Vector3(1000, playerBottom));
		Debug.DrawLine(new Vector3(transform.position.x-collider2D.bounds.extents.x, platformTop), new Vector3(transform.position.x+collider2D.bounds.extents.x, platformTop));

		if(playerBottom < platformTop || Game.main.player.ignorePlatform){
			Physics2D.IgnoreCollision(collider2D, Game.main.player.collider2D, true);
		}else{
			Physics2D.IgnoreCollision(collider2D, Game.main.player.collider2D, false);
		}
	}

	void OnCollisionEnter2D(Collision2D col){

	}
}
