using UnityEngine;
using System.Collections;

public class CloudController : MonoBehaviour {
	public GameObject cloudPrefab;
	public float spawnHeight = 5f;
	public float spawnHeightFlux = 3f;
	public float spawnInterval = 0.5f;
	public float intervalFlux = 0.5f;
	
	private Timer timer; 

	// Use this for initialization
	void Start () {
		timer = gameObject.AddComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(timer > (spawnInterval + (Random.value - 0.5f) * intervalFlux) 
				- Game.main.player.rigidbody2D.velocity.x / Camera.main.orthographicSize){
			Vector3 spawnPos = Camera.main.transform.position;
			spawnPos.x += Camera.main.orthographicSize * 2f;
			spawnPos.y = spawnHeight + (Random.value-0.5f) * spawnHeightFlux;
			spawnPos.z = 1f;

			GameObject obj = (GameObject)Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
			timer.Reset();
		}
	}
}
