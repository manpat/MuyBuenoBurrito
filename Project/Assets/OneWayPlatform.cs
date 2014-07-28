using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour {
	void Update () {
		float platformTop = transform.position.y + collider2D.bounds.extents.y;

		Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - transform.localScale*0.7f, transform.position + transform.localScale*0.7f);
		foreach(Collider2D c in colliders){
			if(c == collider2D || c.rigidbody2D == null) continue;

			float bottom = c.transform.position.y - c.collider2D.bounds.extents.y;

			Physics2D.IgnoreCollision(collider2D, c, bottom < platformTop);
		} 

		float playerBottom = Game.main.player.transform.position.y - Game.main.player.collider2D.bounds.extents.y;

		Debug.DrawLine(new Vector3(-1000, playerBottom), new Vector3(1000, playerBottom));
		Debug.DrawLine(new Vector3(transform.position.x-collider2D.bounds.extents.x, platformTop), 
					new Vector3(transform.position.x+collider2D.bounds.extents.x, platformTop));

		Physics2D.IgnoreCollision(collider2D, Game.main.player.collider2D, (playerBottom < platformTop || Game.main.player.ignorePlatform));
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.green;	
		Gizmos.DrawWireCube(transform.position, transform.localScale*0.7f*2f);
	}
}
