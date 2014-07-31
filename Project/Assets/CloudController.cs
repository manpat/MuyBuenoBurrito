using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {
	public GameObject cloudPrefab;
	public float spawnHeight = 5f;
	public float spawnHeightFlux = 3f;
	public float spawnInterval = 0.5f;
	public float intervalFlux = 0.5f;
	
	private Timer timer; 

	void Start () {
		timer = gameObject.AddComponent<Timer>();
	}
	
	void Update () {
		// print(Game.main.player.rigidbody2D.velocity.x / Camera.main.orthographicSize);
		if(timer > (spawnInterval + (Random.value - 0.5f) * intervalFlux 
				- Game.main.player.rigidbody2D.velocity.x / Camera.main.orthographicSize * 0f)){

			Vector3 spawnPos = Camera.main.transform.position;
			spawnPos.x += Camera.main.orthographicSize * 2f;
			spawnPos.y = spawnHeight + (Random.value-0.5f) * spawnHeightFlux;
			spawnPos.z = 3f;

			Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
			timer.Reset();
		}
	}
}
