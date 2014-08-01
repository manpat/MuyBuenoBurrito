using UnityEngine;
using System.Collections;

public class Enemy_Basic : Enemy {
	public bool isRunning = false;

	private Timer attackTimer;

	protected override void Start () {
		base.Start();
		attackTimer = gameObject.AddComponent<Timer>();
	}
	
	protected override void Update () {
		base.Update();
		if(isDead) return;

		Vector2 vel = rigidbody2D.velocity;
		if(!isRunning){
			isRunning = Mathf.Abs(GetDirToPlayer()) > 0f;
		}else if(Mathf.Abs(DistToPlayer().x) > Game.main.player.collider2D.bounds.extents.x*2f){ // Don't move if too close
			// vel.x = (GetDirToPlayer() * moveSpeed + vel.x*2f)/3f;
			vel.x += (GetDirToPlayer() * moveSpeed - vel.x)*0.1f;
			rigidbody2D.velocity = vel;
		}

		short nDirFacing = dirFacing;
		if(Mathf.Abs(vel.x) > 0f){
			nDirFacing = (short)Mathf.Sign(vel.x);
		}

		if(Mathf.Abs(vel.x) > 0.5f){
			SetAnimationState(EnemyState.Running, nDirFacing);
		}else{
			SetAnimationState(EnemyState.Idle, nDirFacing);
		}
	}

	protected override void Attack(GameObject player){
		if(attackTimer < 1f/attackRate) return;
		
		player.SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);
		attackTimer.Reset();
	}
}
