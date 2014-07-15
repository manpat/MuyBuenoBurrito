using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	static public Game main; // Singleton
	public float deathLevel = -1f; 

	public GameObject playerPrefab;
	public PlayerController player;
	public GameObject playerSpawn;

	void Awake(){
		main = this; // Set singleton
	}

	void Start () {
		RespawnPlayer();
	}
	
	void Update () {
	
	}

	public void RespawnPlayer(){
		if(player != null) Destroy(player.gameObject);

		GameObject obj = (GameObject)Instantiate(playerPrefab);
		obj.transform.position = playerSpawn.transform.position;

		player = obj.GetComponent<PlayerController>();
		player.isDead = false;
	}

	public void PlayerDeath(){ // Should be called when player dies
		print("Player died");
		Invoke("RespawnPlayer", 2f);
		// Show stats or whatever

	}
	public void PlayerEndLevel(){ // Should be called when player reaches end of level
		print("End of level");
		// Load next level?
		// Win screen
	}

	public void EnemyDeath(){ // Should be called when an enemy dies

	}
	public void EnemyDeathByPlayer(){ // Should be called when an enemy is killed by player
		// Add to stats or whatever
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(-1000, deathLevel), new Vector3(1000, deathLevel));

		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(Camera.main.transform.position - Vector3.left * Camera.main.aspect * Camera.main.orthographicSize, 0.5f);
		Gizmos.DrawSphere(Camera.main.transform.position + Vector3.left * Camera.main.aspect * Camera.main.orthographicSize, 0.5f);
		Gizmos.DrawSphere(Camera.main.transform.position + Vector3.up * Camera.main.orthographicSize, 0.5f);
		Gizmos.DrawSphere(Camera.main.transform.position - Vector3.up * Camera.main.orthographicSize, 0.5f);
	}
}
