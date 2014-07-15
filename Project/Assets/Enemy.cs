using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float health = 100f;
	public float attack = 10f;
	public float attackRate = 0.5f; // Hz

	public bool isDead = false;

	private Timer attackTimer;

	void Start () {
		attackTimer = gameObject.AddComponent<Timer>();
	}
	
	void Update () {
		if(isDead) return;

		if(health <= 0f){
			Die();
		}
	}

	void Die(){
		Game.main.EnemyDeath();
		isDead = true;
	}

	void TakeDamage(float amt){
		health -= amt;
		if(health <= 0f) Die();
	}

	void Attack(GameObject player){
		if(attackTimer < 1f/attackRate) return;
		
		player.SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);
		attackTimer.Reset();
	}

	void OnCollisionEnter2D(Collision2D col){
		string tag = col.gameObject.tag;
		if(tag == "Player"){
			Attack(col.gameObject);
		}
	}
	void OnCollisionStay2D(Collision2D col){
		string tag = col.gameObject.tag;
		if(tag == "Player"){
			Attack(col.gameObject);
		}
	}
}
