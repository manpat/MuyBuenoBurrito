using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudController : MonoBehaviour {
	public GameObject cloudPrefab;
	public float spawnHeight = 5f;
	public float spawnHeightFlux = 3f;
	public float spawnInterval = 0.5f;
	public float intervalFlux = 0.5f;

	public float cloudMoveSpeed = 2f;

	public float cloudSpawnX = 50f;
	public float cloudDieX = -50f;
	
	private List<GameObject> clouds = new List<GameObject>();

	void Start () {
		float distInterval = spawnInterval / cloudMoveSpeed;

		for(float x = cloudDieX; x < cloudSpawnX; x += (distInterval + (Random.value - 0.5f) * intervalFlux)){
			clouds.Add(SpawnCloud(new Vector3(x, spawnHeight + (Random.value-0.5f) * spawnHeightFlux, 2f)));
		}
	}
	
	void Update () {
		foreach(GameObject c in clouds){
			c.transform.position += -Vector3.right * cloudMoveSpeed * Time.deltaTime;
			if(c.transform.position.x < cloudDieX){
				c.transform.position += Vector3.right * (cloudSpawnX - cloudDieX);
			}
		}
	}

	GameObject SpawnCloud(Vector3 pos){
		return (GameObject)Instantiate(cloudPrefab, pos, Quaternion.identity);
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.white;
		Gizmos.DrawLine(new Vector3(cloudDieX, -1000f, 0f), new Vector3(cloudDieX, 1000f, 0f));
		Gizmos.DrawLine(new Vector3(cloudSpawnX, -1000f, 0f), new Vector3(cloudSpawnX, 1000f, 0f));
		Gizmos.DrawLine(new Vector3(-1000f, spawnHeight, 0f), new Vector3(1000f, spawnHeight, 0f));
	}

	void OnLevelWasLoaded(int lvl){
		clouds.Clear();
		
		float distInterval = spawnInterval / cloudMoveSpeed;

		for(float x = cloudDieX; x < cloudSpawnX; x += (distInterval + (Random.value - 0.5f) * intervalFlux)){
			clouds.Add(SpawnCloud(new Vector3(x, spawnHeight + (Random.value-0.5f) * spawnHeightFlux, 2f)));
		}
	}
}
