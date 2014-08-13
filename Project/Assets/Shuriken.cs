using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	private Timer deathTimer;

	void Start(){
		deathTimer = gameObject.AddComponent<Timer>();
	}

	void Update(){
		if(deathTimer > 20f) Die();
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.CompareTag("Enemy")){
			col.gameObject.SendMessage("TakeDamage", 100f);

			Die();
		}
	}

	void Die(){
		Destroy(gameObject);
	}
}
