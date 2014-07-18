using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	public GameObject playerSpawn;
	public float deathLevel = -1f; 

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(-1000, deathLevel), new Vector3(1000, deathLevel));

		Gizmos.color = new Color(0.2f, 1f, 1f);
		Gizmos.DrawSphere(playerSpawn.transform.position, 0.5f);
	}
}
