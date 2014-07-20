﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float health = 100f;
	public float speed = 10f;
	public float attack = 10f;
	public float attackRate = 3f; // Hz

	public bool isDead = false;
	public bool isRunning = false;

	private Timer attackTimer;

	void Start () {
		attackTimer = gameObject.AddComponent<Timer>();
	}
	
	void Update () {
		if(isDead) return;

		if(health <= 0f){
			Die();
		}

		if(!isRunning){
			isRunning = Mathf.Abs(GetDirFacingIfPlayerSeen()) > 0f;
		}else if(Mathf.Abs(DistToPlayer().x) > Game.main.player.transform.localScale.x){ // Don't move if too close
			Vector2 vel = rigidbody2D.velocity;
			vel.x = GetDirFacingIfPlayerSeen() * speed;
			rigidbody2D.velocity = vel;
		}
	}

	void Die(){
		Game.main.EnemyDeath();
		isDead = true;
	}

	void TakeDamage(float amt){
		health -= amt;
		if(!isDead && health <= 0f) Die(); // Die if necessary but don't die too much
	}

	Vector2 DistToPlayer(){
		return Game.main.player.transform.position - transform.position;
	}

	float GetDirFacingIfPlayerSeen(){
		float diff = DistToPlayer().x;
		if(Mathf.Abs(diff) < Camera.main.orthographicSize*2f){
			return Mathf.Sign(diff);
		}

		return 0;
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
