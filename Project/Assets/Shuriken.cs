using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.CompareTag("Enemy")){
			col.gameObject.SendMessage("TakeDamage", 100f);

			Die();
		}
	}

	void Die(){
		Destroy(gameObject);
	}
}
