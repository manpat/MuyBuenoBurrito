using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 1f;
	public float jumpHeight = 2f;
	public float airTime = 0.3f;

	public float initialJumpVelocity;
	private bool isGrounded = true;


	// Use this for initialization
	void Start () {
		SetUpGravity();
	}
	
	// Update is called once per frame
	void Update () {
		SetUpGravity(); ////////////////////////// TEMP ///////////

		Vector3 vel = rigidbody2D.velocity;
		vel.x = Input.GetAxis("Horizontal") * moveSpeed;

		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
			vel += Vector3.up * initialJumpVelocity;
			isGrounded = false;
		}else if(Input.GetKeyUp(KeyCode.Space) && vel.y > 0f){
			vel.y *= 0.5f;
		}

		rigidbody2D.velocity = vel;
		CheckHeight();
	}

	void SetUpGravity(){
		float gravity = 8f * jumpHeight / (5f * airTime * airTime); // Fancy math
		Physics2D.gravity = -Vector2.up * gravity;

		initialJumpVelocity = airTime * gravity;
	}

	void CheckHeight(){
		RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position, Vector2.one, 0, -Vector2.up);
		if(hit.fraction < 0.02f){
			isGrounded = true;
		}else{
			isGrounded = false;
		}
	}
}
