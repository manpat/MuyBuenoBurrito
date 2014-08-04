using UnityEngine;
using System.Collections;

public class CactusShard : MonoBehaviour {
	void Start(){
		(gameObject.AddComponent<DeathProxy>()).deathTime = 20f; // Destroy self after 20 seconds
	}

	void Update(){
		if(transform.position.y < Game.main.currentLevel.deathLevel){
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.CompareTag("Player")){
			col.gameObject.SendMessage("TakeDamage", 10f);
			col.gameObject.rigidbody2D.AddForce(Vector2.up * 10f / Time.deltaTime);
		}else if(col.gameObject.CompareTag("Enemy")){
			col.gameObject.SendMessage("TakeDamage", 10f * (rigidbody2D.velocity.magnitude/8f + 1f));
		}
	}
}
