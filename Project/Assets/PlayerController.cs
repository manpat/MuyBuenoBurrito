using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState {
	Idle,
	Running,
	Jumping,
	Falling,
	Attacking,
	Dead,
	// more
};

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 1f;
	public float jumpHeight = 2f; // Must be initialised before SetUpGravity is called 
	public float doubleJumpMultiplier = 1.4f;
	public float airTime = 0.3f; // Must be initialised before SetUpGravity is called

	public float sameLevelJumpDistance; // just for reference
	public float maxHeightJumpDistance; // just for reference
	private float initialJumpVelocity;

	public bool ignorePlatform = false;

	private float _health = 100f;
	public float health {
		get{ return _health; }
		set{
			_health = value;;
			PlayerUI.SetHealthbarValue(_health/100f);
		}
	}

	public bool isGrounded = true;
	public bool isJumping = false;
	public bool isDoubleJumping = false;
	public bool isTripleJumping = false;
	public bool spaceBeingHeld = false;
	public bool isDead = false;

	private Animator animator;
	private Timer animationTimer;
	private PlayerState state = PlayerState.Idle;
	private short dirFacing = 1;
	// private ParticleSystem particleSystem;

	public KeyCode attackKey;
	public KeyCode throwShurikenKey;
	public KeyCode specialKey; // for the leap?
	public float maxAttackDist = 2f;
	public int shurikensRemaining = 3;

	private bool isLeaping = false;
	private Timer leapTimer;
	private Vector3 leapToPos;
	public float leapCooldown = 2f;
	public float leapDist = 2f;
	public float leapDamage = 100f;

	public GameObject shurikenPrefab;
	public GameObject blamoPrefab;

	// Pickup stuff

	public bool canTripleJump = false;
	public float attackMultiplier = 1f;
	public float speedMultiplier = 1f;
	[SerializeField] private List<Color> pickupTints = new List<Color>();
	private Timer pickupTintTimer;

	void Start () {
		SetUpGravity();
		animator = gameObject.GetComponent<Animator>();
		animationTimer = gameObject.AddComponent<Timer>();
		pickupTintTimer = gameObject.AddComponent<Timer>();
		leapTimer = gameObject.AddComponent<Timer>();

		health = 100f;
	}
	
	void Update () {
		if(Game.main.endOfGame || isDead) return;
		PlayerUI.SetNumShurikens(shurikensRemaining);

		// SetUpGravity(); ////////////////////////// TEMP /////////// This is only for realtime tweaking

		if(isLeaping){
			Vector3 diff = (leapToPos - transform.position);
			// transform.position += diff.normalized * Time.deltaTime * leapDist * 8f; 
			rigidbody2D.velocity = diff.normalized * leapDist * 8f; 
			if(diff.magnitude <= leapDist * 0.1f) isLeaping = false;

			CheckHeight();
			MoveCamera();
			DoPickupTint();
			return;
		}

		Vector3 vel = rigidbody2D.velocity;
		vel.x = Input.GetAxis("Horizontal") * moveSpeed * speedMultiplier;

		ignorePlatform = false;

		if(Input.GetKey(KeyCode.Space) && Input.GetAxis("Vertical") < -0.2f){ // Drop from platform
			ignorePlatform = true;
			spaceBeingHeld = true;

			// platformDropTimer.Reset();

		}else if(Input.GetKey(KeyCode.Space) && (isGrounded || 
			(!spaceBeingHeld && !(isDoubleJumping && !canTripleJump || isTripleJumping)
			&& rigidbody2D.velocity.y <= 0f))){ // Jump

			// Jump if space is pressed and the player is either on the ground, or has released space previously, hasn't double
			//		jumped yet, and is falling. Note: player will start falling almost immediately after space is released. 

			vel.y = initialJumpVelocity * (isJumping?doubleJumpMultiplier:1f);
			spaceBeingHeld = true;

			if(isGrounded){
				isGrounded = false;
				isJumping = true;
			}else if(!isDoubleJumping){
				isDoubleJumping = true;
			}else if(!isTripleJumping){
				isTripleJumping = true;
			}

		}else if(Input.GetKeyUp(KeyCode.Space)/* && vel.y > 0f*/){ // Stop jumping if player is moving up still
			// vel.y *= 0.5f; // Halve upwards velocity
			spaceBeingHeld = false;
		}

		rigidbody2D.velocity = vel;

		short nDirFacing = dirFacing;
		if(Mathf.Abs(vel.x) > 0f){
			nDirFacing = (short)Mathf.Sign(vel.x);
		}

		if(Input.GetKeyDown(attackKey)){
			Attack();
			SetAnimationState(PlayerState.Attacking, nDirFacing);

		}else if(Input.GetKeyDown(throwShurikenKey) && shurikensRemaining > 0){
			ThrowShuriken();
			SetAnimationState(PlayerState.Attacking, nDirFacing);

		}else if(Input.GetKeyDown(specialKey) && leapTimer > leapCooldown){
			LeapAttack();
			SetAnimationState(PlayerState.Jumping, nDirFacing);

		}else if(isJumping || isDoubleJumping || isTripleJumping){
			if(vel.y < -Physics2D.gravity.magnitude*rigidbody2D.gravityScale * airTime/3f) {
				SetAnimationState(PlayerState.Falling, nDirFacing); // Falling
			}else{
				SetAnimationState(PlayerState.Jumping, nDirFacing);
			}
		}else if(isGrounded && Mathf.Abs(vel.x) > 0.5f){
			SetAnimationState(PlayerState.Running, nDirFacing);
		}else{
			SetAnimationState(PlayerState.Idle, nDirFacing);
		}

		CheckHeight();
		MoveCamera();
		DoPickupTint();
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

		Vector2 castBoxSize = collider2D.bounds.extents*2f;
		castBoxSize.y = 0.1f;

		RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position, castBoxSize, 0, -Vector2.up); // Raycast down
		if(hit && hit.fraction <= (collider2D.bounds.extents.y + 0.1f)){ // If raycast returned something and distance is <= player height/2
			// on ground
			isGrounded = true;
			isJumping = false;
			isDoubleJumping = false;
			isTripleJumping = false;
			Debug.DrawLine(transform.position, hit.point, Color.green);
		}else{
			isGrounded = false;
			Debug.DrawLine(transform.position, hit.point, Color.red);
		}
	}

	void Die(){
		Game.main.PlayerDeath();
		isDead = true;
		SetAnimationState(PlayerState.Dead, dirFacing);

		rigidbody2D.AddForce(Vector2.up * 50f / Time.deltaTime); // Mario death
		collider2D.enabled = false;
	}

	void TakeDamage(float amt){
		health -= amt;
		// NEEDS FEEDBACK
		// rigidbody2D.AddForce((Vector2.up * 10f - rigidbody2D.velocity * 30f) / Time.deltaTime);

		Game.main.AddStat("DamageTaken", amt);
		Game.main.CreateBlamo(transform.position, amt);
		if(!isDead && health <= 0f) Die(); // Die if necessary but don't die too much
	}

	void Attack(){
		Vector2 castBoxSize = collider2D.bounds.extents*2f; // Size of player
		RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, castBoxSize, 0, Vector2.right * (float)dirFacing, maxAttackDist/*, LayerMask.GetMask("Enemy")*/);
		if(hits.Length > 0){
			foreach(RaycastHit2D hit in hits){
				if(hit.rigidbody) hit.rigidbody.AddForce(Vector2.right * (float)dirFacing * 50f / Time.deltaTime * attackMultiplier);
				hit.collider.gameObject.SendMessage("TakeDamage", 50f * attackMultiplier, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void ThrowShuriken(){
		if(shurikensRemaining <= 0) return;

		GameObject obj = (GameObject)Instantiate(shurikenPrefab, transform.position, Quaternion.identity);
		Physics2D.IgnoreCollision(collider2D, obj.collider2D, true);
		obj.rigidbody2D.velocity = Vector2.right * (float)dirFacing * 30f;
		obj.rigidbody2D.angularVelocity = -360f * 3f;

		--shurikensRemaining;
	}

	void LeapAttack(){
		Vector2 boxcastSize = collider2D.bounds.extents*3f;

		float angle = Mathf.PI/18f;

		if(!isGrounded) angle = -angle/4f;

		Vector3 dir = Vector3.right * (float)dirFacing * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);

		RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxcastSize, 0f, dir, leapDist, LayerMask.GetMask("Enemy", "Crates"));
		foreach(RaycastHit2D hit in hits){
			if(hit.rigidbody) hit.rigidbody.AddForce(Vector2.up * 10f / Time.deltaTime * attackMultiplier);
			hit.collider.gameObject.SendMessage("TakeDamage", leapDamage/(float)hits.Length * attackMultiplier, SendMessageOptions.DontRequireReceiver);
		}

		leapToPos = transform.position + dir * leapDist;
		isLeaping = true;
		leapTimer.Reset();
	}

	void SetAnimationState(PlayerState newState, short newDirFacing){
		AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

		// If the animation doesn't loop, wait until it finishes
		if((!asi.loop && animationTimer < asi.length) || state == newState && dirFacing == newDirFacing) return;
		animationTimer.Reset();

		state = newState;
		dirFacing = newDirFacing;

		switch(newState){
			case PlayerState.Idle:
				animator.Play("idle");
				break;
			case PlayerState.Running:
				animator.Play("run");
				break;
			case PlayerState.Jumping:
				animator.Play("jump");
				break;
			case PlayerState.Falling:
				animator.Play("fall");
				break;
			case PlayerState.Attacking:
				animator.Play("attack");
				break;
			case PlayerState.Dead:
				animator.Play("damage");
				break;
		}

		Vector3 scale = transform.localScale;
		scale.x = dirFacing*2f;
		transform.localScale = scale;
	}

	void OnCollisionEnter2D(Collision2D col){
		if(isLeaping) {
			isLeaping = false;
		}
	}

	void DoPickupTint(){
		if(pickupTints.Count == 0) {
			renderer.material.color = Color.white;
			PlayerUI.SetVignetteIntensity(0f);
			PlayerUI.SetVignetteColor(Color.white);
			return;
		}

		int count = pickupTints.Count;

		int idx1 = Mathf.FloorToInt(pickupTintTimer) % count;
		int idx2 = Mathf.FloorToInt(pickupTintTimer+1) % count;
		float a = pickupTintTimer - Mathf.FloorToInt(pickupTintTimer);

		Color c1 = pickupTints[idx1];
		Color c2 = pickupTints[idx2];
		Color c = Color.Lerp(c1, c2, a);

		renderer.material.color = c;
		PlayerUI.SetVignetteColor(c);
		PlayerUI.SetVignetteIntensity(Mathf.PingPong(pickupTintTimer, 0.8f)+0.2f);
	}

	public void CreateParticles(Color color){
		particleSystem.startColor = color;
		particleSystem.Emit(500);
	} 

	public void AddPickupTint(Color color){
		pickupTints.Add(color);
	}
	public void RemovePickupTint(Color color){
		pickupTints.RemoveAll(x => (x == color));
	}
}
