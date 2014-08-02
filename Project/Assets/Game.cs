﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	static public Game main; // Singleton

	public GameObject playerPrefab;
	public PlayerController player;

	public GameObject blamoPrefab;

	public Level currentLevel;

	void Awake(){
		main = this; // Set singleton
		DontDestroyOnLoad(this);
	}

	void Start () {
		currentLevel = FindObjectOfType<Level>();
		RespawnPlayer();
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.Return)){
			PlayerEndLevel();
		}
	}

	public Blamo CreateBlamo(Vector2 pos, float dmg){
		Blamo blamo = ((GameObject)Instantiate(blamoPrefab, pos, Quaternion.identity)).GetComponent<Blamo>();
		blamo.lifeTime = 2f;
		blamo.explodeTime = 0.25f;
		blamo.explodeScale = 0.5f + dmg/50f;
		return blamo;
	}

	public void RespawnPlayer(){
		if(player != null) Destroy(player.gameObject);

		GameObject obj = (GameObject)Instantiate(playerPrefab);
		obj.transform.position = currentLevel.playerSpawn.transform.position;

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
		Application.LoadLevel("testlevel2");
	}

	public void EnemyDeath(){ // Should be called when an enemy dies
		print("Enemy died");
	}

	void OnLevelWasLoaded(int level){
		print("Level loaded: " + level.ToString());
		currentLevel = FindObjectOfType<Level>();
		RespawnPlayer();
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(Camera.main.transform.position, 
			new Vector3(Camera.main.aspect * Camera.main.orthographicSize * 2f, Camera.main.orthographicSize * 2f, 0.5f));
		Gizmos.DrawSphere(Camera.main.transform.position - Vector3.left * Camera.main.aspect * Camera.main.orthographicSize, 0.5f);
		Gizmos.DrawSphere(Camera.main.transform.position + Vector3.left * Camera.main.aspect * Camera.main.orthographicSize, 0.5f);
		Gizmos.DrawSphere(Camera.main.transform.position + Vector3.up * Camera.main.orthographicSize, 0.5f);
		Gizmos.DrawSphere(Camera.main.transform.position - Vector3.up * Camera.main.orthographicSize, 0.5f);
	}
}
