using UnityEngine;
using System.Collections;

public class Cactus : Enemy {
	public GameObject cactusJuicePickupPrefab;
	public GameObject cactusShardPrefab;
	public int numShardsSpawnedUponDeath = 2;

	protected override void OnDeath(){
		Instantiate(cactusJuicePickupPrefab, transform.position, Quaternion.identity);

		for(int i = 0; i < numShardsSpawnedUponDeath; ++i){
			Vector3 dir = (Vector3)Random.insideUnitCircle.normalized;
			GameObject obj = (GameObject)Instantiate(cactusShardPrefab, transform.position + dir * 3f, Quaternion.identity);
			obj.rigidbody2D.velocity = dir * 5f;
		}
	}
}
