using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 1f;
	public float jumpHeight = 2f; // Must be initialised before SetUpGravity is called 
	public float airTime = 0.3f; // Must be initialised before SetUpGravity is called

	public float sameLevelJumpDistance; // just for reference
	public float maxHeightJumpDistance; // just for reference
	private float initialJumpVelocity;

	public bool ignorePlatform = false;

	public float health = 100f;

	private bool isGrounded = true;
	private bool isDoubleJumping = false;
	private bool spaceBeingHeld = false;
	public bool isDead = false;

	void Start () {
		SetUpGravity();
	}
	
	void Update () {
		if(isDead) return;

		SetUpGravity(); ////////////////////////// TEMP /////////// This is only for realtime tweaking

		Vector3 vel = rigidbody2D.velocity;
		vel.x = Input.GetAxis("Horizontal") * moveSpeed;

		ignorePlatform = false;

		if(Input.GetKey(KeyCode.Space) && Input.GetAxis("Vertical") < 0f){ // Drop from platform
			ignorePlatform = true;
			spaceBeingHeld = true;

		}else if(Input.GetKey(KeyCode.Space) && (isGrounded || (!spaceBeingHeld && !isDoubleJumping && rigidbody2D.velocity.y < 0f))){ // Jump
			// Jump if space is pressed and the player is either on the ground, or has released space previously, hasn't double
			//		jumped yet, and is falling. Note: player will start falling almost immediately after space is released. 

			vel.y = initialJumpVelocity;
			spaceBeingHeld = true;

			if(isGrounded){
				isGrounded = false;
			}else{
				isDoubleJumping = true;
			}

		}else if(Input.GetKeyUp(KeyCode.Space) && vel.y > 0f){ // Stop jumping if player is moving up still
			vel.y *= 0.5f; // Halve upwards velocity
			spaceBeingHeld = false;
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
		rigidbody2D.gravityScale = gravity / Physics2D.gravity.magnitude;

		initialJumpVelocity = airTime * gravity / 2f;

		sameLevelJumpDistance = moveSpeed * airTime;
		maxHeightJumpDistance = moveSpeed * airTime / 2f;
	}

	void MoveCamera(){
		Vector3 campos = Camera.main.transform.position;

		campos.x = transform.position.x;
		campos.y = Mathf.Max(transform.position.y, Game.main.currentLevel.deathLevel + Camera.main.orthographicSize); // Don't go below death level

		Camera.main.transform.position = campos;
	}

	void CheckHeight(){
		if(transform.position.y < Game.main.currentLevel.deathLevel) Die();

		RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position, Vector2.one, 0, -Vector2.up); // Raycast down
		if(hit && hit.fraction <= (transform.localScale.y/2 + 0.1f)){ // If raycast returned something and distance is <= player height/2
			// on ground
			isGrounded = true;
			isDoubleJumping = false;
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
		// NEEDS FEEDBACK
		// rigidbody2D.AddForce((Vector2.up * 10f - rigidbody2D.velocity * 30f) / Time.deltaTime);

		if(!isDead && health <= 0f) Die(); // Die if necessary but don't die too much
	}
}
