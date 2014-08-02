﻿using UnityEngine;
using System.Collections;

public class Enemy_Advanced : Enemy {
	public float jumpHeight = 2f; // Must be initialised before SetUpGravity is called 
	public float airTime = 0.3f; // Must be initialised before SetUpGravity is called

	public float sameLevelJumpDistance; // just for reference
	public float maxHeightJumpDistance; // just for reference
	private float initialJumpVelocity;

	public bool isRunning = false;
	public bool isGrounded = false;
	public bool isJumping = false;

	private Timer attackTimer;

	private Vector2 velocity;

	protected override void Start () {
		base.Start();
		attackTimer = gameObject.AddComponent<Timer>();

		SetUpGravity();
	}
	
	protected override void Update () {
		base.Update();
		if(isDead) return;
		
		if(!isRunning){
			isRunning = Mathf.Abs(GetDirToPlayer()) > 0f;
		}

		if(GetDirToPlayer() != 0f) MoveTowardsPlayer();
		CheckHeight();

		short nDirFacing = dirFacing;
		if(Mathf.Abs(velocity.x) > 0f){
			nDirFacing = (short)Mathf.Sign(velocity.x);
		}

		if(isJumping/* || isDoubleJumping*/){
			// if(velocity.y < -Physics2D.gravity.magnitude*rigidbody2D.gravityScale * 0.5f) 
					// SetAnimationState(EnemyState.Falling); // Falling for 1/2 a second

			// SetAnimationState(EnemyState.Jumping, nDirFacing);
		}else if(isGrounded && Mathf.Abs(velocity.x) > 0.5f){
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

	void MoveTowardsPlayer(){
		velocity = rigidbody2D.velocity;
		Vector2 playerPos = Game.main.player.transform.position;
		float heightDiff = playerPos.y - transform.position.y;
		float myHeight = collider2D.bounds.extents.y*2f;

		ignorePlatform = false;
		if(heightDiff > myHeight*0.5f && JumpablePlatformAbove()){
			if(isGrounded /*&& !isJumping*/){
				velocity.y = initialJumpVelocity;
	
				isJumping = true;
				isGrounded = false;
				ignorePlatform = false;
			}
		}else if(heightDiff < -myHeight*0.5f && JumpablePlatformBelow()){
			ignorePlatform = true;
		}

		JumpablePlatformBelow();

		Debug.DrawLine((Vector3)playerPos, (Vector3)transform.position, Color.green);

		if(Mathf.Abs(playerPos.x - transform.position.x) < collider2D.bounds.extents.x){
			velocity.x = 0f;
		}else if(WalkableAhead()){
			// velocity.x = (GetDirToPlayer() * moveSpeed + velocity.x*2f)/3f;
			velocity.x += (GetDirToPlayer() * moveSpeed - velocity.x)*0.5f;
		}else{
			velocity.x = 0; //GetDirToPlayer() * moveSpeed;
		}

		rigidbody2D.velocity = velocity;
	}

	float CheckFloorAt(Vector2 pos){
		Vector2 castBoxSize = collider2D.bounds.extents*2f;
		castBoxSize.y = 0.1f;

		RaycastHit2D hit = Physics2D.BoxCast(pos, castBoxSize, 0, -Vector2.up, Mathf.Infinity, ~LayerMask.GetMask("Enemy")); // Raycast down

		if(hit) Debug.DrawLine((Vector3)pos, (Vector3)pos-Vector3.up*hit.fraction, Color.green);
		else Debug.DrawLine((Vector3)pos, (Vector3)pos-Vector3.up*100f, Color.red);

		if(!hit) return Mathf.Infinity;

		return hit.fraction;
	}

	bool WalkableAhead(){
		Vector2 forwardPos = (Vector2)transform.position 
			+ Vector2.right * GetDirToPlayer() * collider2D.bounds.extents.x;
		return CheckFloorAt(forwardPos) < jumpHeight;
	}

	bool JumpablePlatformAbove(){
		Vector2 jumpPeakPosition = (Vector2)transform.position 
			+ Vector2.up * jumpHeight 
			+ Vector2.right * maxHeightJumpDistance * GetDirToPlayer();
		return CheckFloorAt(jumpPeakPosition) < jumpHeight;
	}

	bool JumpablePlatformBelow(){
		Vector2 jumpPeakPosition = (Vector2)transform.position 
			- Vector2.up * collider2D.bounds.extents.y*1.01f
			+ Vector2.right * maxHeightJumpDistance * GetDirToPlayer();
		return CheckFloorAt(jumpPeakPosition) < jumpHeight*2f;
	}

	void CheckHeight(){
		if(CheckFloorAt((Vector2)transform.position) <= (collider2D.bounds.extents.y + 0.1f)){ // If raycast returned something and distance is <= height/2
			// on ground
			isGrounded = true;
			isJumping = false;
		}else{
			isGrounded = false;
		}
	}

	void SetUpGravity(){
		// s = ut + (at^2) /2
		// 0 = u*airTime + gravity*airTime^2 * 1/2
		// u*airTime = -gravity*airTime^2 * 1/2
		// u = -gravity * airTime * 1/2

		// s = ut + at^2 * 1/2
		// jumpHeight = (-gravity * airTime * 1/2) * (airTime * 1/2) + gravity * (airTime * 1/2)^2 * 1/2
		// jumpHeight = -gravity * 1/4 * airTime^2 + gravity * airTime^2 * 1/4 * 1/2
		// jumpHeight = -gravity * airTime^2 * 1/4 + gravity * airTime^2 * 1/8
		// jumpHeight = gravity * airTime^2 * (1/8 - 1/4) = -gravity * airTime^2 * 1/8
		// jumpHeight = -gravity * airTime^2 * 1/8
		// -gravity   = jumpHeight / (airTime^2 * 1/8)
		//  gravity   = -jumpHeight / (airTime^2 * 1/8)
		//  gravity   = -jumpHeight * 8 / airTime^2

		float gravity = 8f * jumpHeight / (airTime * airTime);
		rigidbody2D.gravityScale = gravity / Physics2D.gravity.magnitude;

		initialJumpVelocity = airTime * gravity / 2f;

		sameLevelJumpDistance = moveSpeed * airTime;
		maxHeightJumpDistance = moveSpeed * airTime / 2f;
	}
}