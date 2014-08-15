using UnityEngine;
using System.Collections;

public class CactusShard : MonoBehaviour {
	public float damage = 5f;

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
			col.gameObject.SendMessage("TakeDamage", damage);
			col.gameObject.rigidbody2D.AddForce(Vector2.up * 20f / Time.deltaTime);
		}else if(col.gameObject.CompareTag("Enemy")){
			col.gameObject.SendMessage("TakeDamage", damage * (rigidbody2D.velocity.magnitude/7f + 1f));
		}
	}
}
