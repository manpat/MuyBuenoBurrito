using UnityEngine;
using System.Collections;

public enum EnemyState {
	Idle,
	Running, 
	Jumping,
	Throwing,
};

public class Enemy : MonoBehaviour {
	public float health = 100f;
	public float speed = 10f;
	public float attack = 10f;
	public float attackRate = 3f; // Hz

	public bool isDead = false;
	public bool isRunning = false;

	private Timer attackTimer;

	private Animator animator;
	private EnemyState state = EnemyState.Idle;
	private short dirFacing = 1;

	void Start () {
		attackTimer = gameObject.AddComponent<Timer>();
		animator = gameObject.GetComponent<Animator>();
	}
	
	void Update () {
		if(isDead) return;

		if(health <= 0f){
			Die();
		}

		Vector2 vel = rigidbody2D.velocity;
		if(!isRunning){
			isRunning = Mathf.Abs(GetDirFacingIfPlayerSeen()) > 0f;
		}else if(Mathf.Abs(DistToPlayer().x) > Game.main.player.collider2D.bounds.extents.x*2f){ // Don't move if too close
			vel.x = GetDirFacingIfPlayerSeen() * speed;
			rigidbody2D.velocity = vel;
		}

		short nDirFacing = dirFacing;
		if(Mathf.Abs(vel.x) > 0f){
			nDirFacing = (short)Mathf.Sign(vel.x);
		}

		/*if(isJumping || isDoubleJumping){
			// if(vel.y < -Physics2D.gravity.magnitude*rigidbody2D.gravityScale * 0.5f) 
			//		SetAnimationState(EnemyState.Falling); // Falling for 1/2 a second

			SetAnimationState(EnemyState.Jumping, nDirFacing);
		}else */if(/*isGrounded && */Mathf.Abs(vel.x) > 0.5f){
			SetAnimationState(EnemyState.Running, nDirFacing);
		}else{
			SetAnimationState(EnemyState.Idle, nDirFacing);
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

	void SetAnimationState(EnemyState newState, short newDirFacing){
		if(state == newState && dirFacing == newDirFacing) return;
		state = newState;
		dirFacing = newDirFacing;

		switch(newState){
			case EnemyState.Idle:
				animator.Play("idle");
				break;
			case EnemyState.Running:
				animator.Play("run");
				break;
			case EnemyState.Jumping:
				// animator.Play("jump");
				break;

			default:
			break;
		}

		Vector3 scale = transform.localScale;
		scale.x = dirFacing*2f;
		transform.localScale = scale;
	}
}
