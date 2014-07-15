using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 1f;
	public float jumpHeight = 2f; // Must be initialised before SetUpGravity is called 
	public float airTime = 0.3f; // Must be initialised before SetUpGravity is called

	public float sameLevelJumpDistance; // just for reference
	public float maxHeightJumpDistance; // just for reference
	private float initialJumpVelocity;

	public float health = 100f;

	private bool isGrounded = true;
	public bool isDead = false;

	void Start () {
		SetUpGravity();
	}
	
	void Update () {
		if(isDead) return;

		SetUpGravity(); ////////////////////////// TEMP /////////// This is only for realtime tweaking

		Vector3 vel = rigidbody2D.velocity;
		vel.x = Input.GetAxis("Horizontal") * moveSpeed;

		if(Input.GetKey(KeyCode.Space) && isGrounded){
			vel.y = initialJumpVelocity;
			isGrounded = false;
		}else if(Input.GetKeyUp(KeyCode.Space) && vel.y > 0f){
			vel.y *= 0.5f; // Halve upwards velocity
		}

		rigidbody2D.velocity = vel;

		CheckHeight();
		MoveCamera();
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
		Physics2D.gravity = -Vector2.up * gravity;

		initialJumpVelocity = airTime * gravity / 2f;

		sameLevelJumpDistance = moveSpeed * airTime;
		maxHeightJumpDistance = moveSpeed * airTime / 2f;
	}

	void MoveCamera(){
		Vector3 campos = Camera.main.transform.position;

		campos.x = transform.position.x;
		campos.y = Mathf.Max(transform.position.y, Game.main.deathLevel + Camera.main.orthographicSize);

		Camera.main.transform.position = campos;
	}

	void CheckHeight(){
		if(transform.position.y < Game.main.deathLevel) Die();

		RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position, Vector2.one, 0, -Vector2.up);
		if(hit && hit.fraction < (transform.localScale.y/2 + 0.1f)){ // Accounts for size of body
			isGrounded = true;
			Debug.DrawLine(transform.position, hit.point, Color.green);
		}else{
			isGrounded = false;
			Debug.DrawLine(transform.position, hit.point, Color.red);
		}
	}

	void Die(){
		Game.main.PlayerDeath();
		isDead = true;
	}

	void TakeDamage(float amt){
		health -= amt;
		if(health <= 0f) Die();
	}
}
