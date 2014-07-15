using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	static public Game main; // Singleton
	public float deathLevel = -1f; 

	void Awake(){
		main = this; // Set singleton
	}

	void Start () {
		
	}
	
	void Update () {
	
	}

	public void PlayerDeath(){ // Should be called when player dies
		print("Player died");
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
		Gizmos.DrawLine(new Vector3(-100, deathLevel), new Vector3(100, deathLevel));
	}
}
