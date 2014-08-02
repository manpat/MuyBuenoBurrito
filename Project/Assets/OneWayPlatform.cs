using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour {
	void Start(){
		ModifyMaterial();
	}

	void Update () {
		Vector2 thisScale = collider2D.bounds.extents;
		float platformTop = transform.position.y + thisScale.y;

		Collider2D[] colliders = Physics2D.OverlapAreaAll((Vector2)transform.position - thisScale*2f*0.7f, (Vector2)transform.position + thisScale*2f*0.7f);
		foreach(Collider2D c in colliders){
			if(c == collider2D || c.rigidbody2D == null) continue;

			float bottom = c.transform.position.y - c.collider2D.bounds.extents.y;
			bool ignorePlatform = bottom < platformTop;

			if(c.CompareTag("Enemy")){
				ignorePlatform = ignorePlatform || c.GetComponent<Enemy>().ignorePlatform;
			}

			Physics2D.IgnoreCollision(collider2D, c, ignorePlatform);
		} 

		float playerBottom = Game.main.player.transform.position.y - Game.main.player.collider2D.bounds.extents.y;

		Debug.DrawLine(new Vector3(-1000, playerBottom), new Vector3(1000, playerBottom));
		Debug.DrawLine(new Vector3(transform.position.x-thisScale.x, platformTop), 
					new Vector3(transform.position.x+thisScale.x, platformTop));

		Physics2D.IgnoreCollision(collider2D, Game.main.player.collider2D, (playerBottom < platformTop || Game.main.player.ignorePlatform));
	}

	void ModifyMaterial(){
		float grey = 1f - transform.position.z/11f;
		renderer.material.color = new Color(grey, grey, grey);
	}

	void OnDrawGizmos(){
		// ModifyMaterial();
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.green;	
		Gizmos.DrawWireCube(transform.position, collider2D.bounds.extents*4f*0.7f);
	}
}
