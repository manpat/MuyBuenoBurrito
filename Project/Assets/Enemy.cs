using UnityEngine;
using System.Collections;

public enum EnemyState {
	Idle,
	Running, 
	Jumping,
	Falling,
	Attacking,
	Throwing,
};

public class Enemy : MonoBehaviour {
	public float health = 100f;
	public float moveSpeed = 10f;
	public float attack = 10f;
	public float attackRate = 3f; // Hz

	protected Timer attackTimer;

	public bool isDead = false;
	public bool isAttacking = false;
	public bool ignorePlatform = false;

	public float deathTime = 2f;

	[SerializeField] protected AudioClip onDeathClip;

	protected Animator animator;
	protected Timer animationTimer;
	protected EnemyState state = EnemyState.Idle;
	protected short dirFacing = 1;

	protected virtual void Start () {
		animator = gameObject.GetComponent<Animator>();
		animationTimer = gameObject.AddComponent<Timer>();
		attackTimer = gameObject.AddComponent<Timer>();
	}
	
	protected virtual void Update () {
		if(Game.main.endOfGame || isDead) return;

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
		if(isDead) return;
		isDead = true;
		Game.main.EnemyDeath();

		if(onDeathClip) AudioSource.PlayClipAtPoint(onDeathClip, transform.position, 0.5f);
		if(rigidbody2D) rigidbody2D.velocity = Vector2.up * 10f;
		collider2D.enabled = false;

		OnDeath();

		if(deathTime == 0f){
			Destroy(gameObject);
		}else{
			(gameObject.AddComponent<DeathProxy>()).deathTime = deathTime; // Destroy gameobject after 2 seconds
		}
	}

	protected virtual void OnDeath(){
		// Do particle effects here
	}

	protected void TakeDamage(float dmg){
		if(isDead) return;

		health -= dmg;
		if(health <= 0f) Die(); // Die if necessary but don't die too much
		Game.main.AddStat("DamageDealt", dmg);

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
		if(isDead || attackTimer < 1f/attackRate) return;
		isAttacking = true;
		
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

	protected void SetAnimationState(EnemyState newState, short newDirFacing, bool forceChange = false){
		if(!animator) return;
		AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

		// If the animation doesn't loop, wait until it finishes unless forceChange is set
		if((!asi.loop && animationTimer < asi.length && !forceChange) || state == newState && dirFacing == newDirFacing) return;
		animationTimer.Reset();

		state = newState;
		dirFacing = newDirFacing;

		switch(newState){
			case EnemyState.Idle:
				animator.Play("idle");
				// print("SetAnimationState idle");
				break;
			case EnemyState.Running:
				animator.Play("run");
				// print("SetAnimationState run");
				break;
			case EnemyState.Jumping:
				animator.Play("jump");
				// print("SetAnimationState jump");
				break;
			case EnemyState.Falling:
				animator.Play("fall");
				// print("SetAnimationState fall");
				break;
			case EnemyState.Attacking:
				animator.Play("attack");
				// print("SetAnimationState attack");
				break;
			case EnemyState.Throwing:
				animator.Play("throw");
				// print("SetAnimationState throw");
				break;

			default:
			break;
		}

		Vector3 scale = transform.localScale;
		scale.x = dirFacing*2f;
		transform.localScale = scale;
	}
}
