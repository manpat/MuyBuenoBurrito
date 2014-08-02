using UnityEngine;
using System.Collections;

public enum EnemyState {
	Idle,
	Running, 
	Jumping,
	Attacking,
	Throwing,
};

public class Enemy : MonoBehaviour {
	public float health = 100f;
	public float moveSpeed = 10f;
	public float attack = 10f;
	public float attackRate = 3f; // Hz

	private Timer attackTimer;

	public bool isDead = false;
	public bool isAttacking = false;
	public bool ignorePlatform = false;

	public GameObject deathSpawnPrefab;

	protected Animator animator;
	private Timer animationTimer;
	protected EnemyState state = EnemyState.Idle;
	protected short dirFacing = 1;

	protected virtual void Start () {
		animator = gameObject.GetComponent<Animator>();
		animationTimer = gameObject.AddComponent<Timer>();
		attackTimer = gameObject.AddComponent<Timer>();
	}
	
	protected virtual void Update () {
		if(isDead) return;

		if(health <= 0f){
			Die();
		}
		
		if(transform.position.y < Game.main.currentLevel.deathLevel) Die();

		if(attackTimer > 1f/attackRate){
			isAttacking = false;
		}else if(isAttacking){
			SetAnimationState(EnemyState.Attacking, dirFacing);
		}
	}

	protected void Die(){
		Game.main.EnemyDeath();
		if(deathSpawnPrefab) Instantiate(deathSpawnPrefab, transform.position, Quaternion.identity); // For particle effects and such
		isDead = true;

		(gameObject.AddComponent<DeathProxy>()).deathTime = 2f; // Destroy gameobject after 2 seconds
		if(rigidbody2D) rigidbody2D.velocity = Vector2.up * 10f;
	}

	protected void TakeDamage(float dmg){
		health -= dmg;
		if(!isDead && health <= 0f) Die(); // Die if necessary but don't die too much

		Game.main.CreateBlamo(transform.position, dmg);
	}

	protected Vector2 DistToPlayer(){
		return Game.main.player.transform.position - transform.position;
	}

	protected float GetDirToPlayer(){
		float diff = DistToPlayer().x;
		if(Mathf.Abs(diff) < Camera.main.orthographicSize*2f){
			return Mathf.Sign(diff);
		}

		return 0;
	}

	protected virtual void Attack(GameObject player){
		isAttacking = true;
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

	protected void SetAnimationState(EnemyState newState, short newDirFacing){
		if(!animator) return;
		AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

		// If the animation doesn't loop, wait until it finishes
		if((!asi.loop && animationTimer < asi.length) || state == newState && dirFacing == newDirFacing) return;
		animationTimer.Reset();

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
				animator.Play("jump");
				break;
			case EnemyState.Attacking:
				animator.Play("attack");
				break;

			default:
			break;
		}

		Vector3 scale = transform.localScale;
		scale.x = dirFacing*2f;
		transform.localScale = scale;
	}
}
