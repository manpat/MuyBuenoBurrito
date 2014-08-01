using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col){
		if(col.CompareTag("Player")){
			Game.main.PlayerEndLevel();
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = new Color(0.1f, 1f, 0.8f, 0.6f);
		Gizmos.DrawCube(transform.position, collider2D.bounds.extents*2f);
		Gizmos.color = new Color(0.2f, 1f, 0.5f, 1f);
		Gizmos.DrawWireCube(transform.position, collider2D.bounds.extents*2f);
	}
}
