using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	static public Game main; // Singleton

	public GameObject playerPrefab;
	public PlayerController player;

	public GameObject blamoPrefab;
	public GameObject[] pickupPrefabs;

	public Level currentLevel;

	private Dictionary<string, float> stats = new Dictionary<string, float>();

	public bool endOfGame = false;

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

	public void CreateRandomPickup(Vector2 pos){
		Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Length-1)], pos, Quaternion.identity);
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
		switch(Application.loadedLevelName){
			case "level1":
				Application.LoadLevel("level2");
				break;

			case "level2":
				endOfGame = true;
				Application.LoadLevel("endgame");
				break;
		}
	}

	public void EnemyDeath(){ // Should be called when an enemy dies
		print("Enemy died");
		IncStat("EnemiesKilled");
	}

	void OnLevelWasLoaded(int level){
		print("Level loaded: " + level.ToString());

		if(endOfGame)  return;
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

	public void SetStat(string _name, float _amount){
		stats[_name] = _amount;
	}
	public void AddStat(string _name, float _amount){
		if(!stats.ContainsKey(_name)){
			stats[_name] = 0f;
		}
		stats[_name] += _amount;
	}
	public void IncStat(string _name){
		AddStat(_name, 1f);
	}
	public void DecStat(string _name){
		AddStat(_name, -1f);
	}

	public float GetStat(string _name){
		if(!stats.ContainsKey(_name)){
			stats[_name] = 0f;
		}

		return stats[_name];
	}

	public float CalcScore(){
		float score = 0f;

		score += GetStat("DamageDealt");
		score += GetStat("EnemiesKilled")*50f;
		score += GetStat("PickupsGot")*100f;
		score -= GetStat("DamageTaken");

		return score;
	}
}
