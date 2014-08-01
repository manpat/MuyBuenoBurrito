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
	public float moveSpeed = 10f;
	public float attack = 10f;
	public float attackRate = 3f; // Hz

	public bool isDead = false;
	public bool ignorePlatform = false;

	public GameObject deathProxy;

	protected Animator animator;
	protected EnemyState state = EnemyState.Idle;
	protected short dirFacing = 1;

	protected virtual void Start () {
		animator = gameObject.GetComponent<Animator>();
	}
	
	protected virtual void Update () {
		if(isDead) return;

		if(health <= 0f){
			Die();
		}
		
		if(transform.position.y < Game.main.currentLevel.deathLevel) Die();
	}

	protected void Die(){
		Game.main.EnemyDeath();
		if(deathProxy) Instantiate(deathProxy, transform.position, Quaternion.identity);
		isDead = true;
		Destroy(gameObject);
	}

	protected void TakeDamage(float amt){
		health -= amt;
		if(!isDead && health <= 0f) Die(); // Die if necessary but don't die too much
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
				animator.Play("jump");
				break;

			default:
			break;
		}

		Vector3 scale = transform.localScale;
		scale.x = dirFacing*2f;
		transform.localScale = scale;
	}
}
