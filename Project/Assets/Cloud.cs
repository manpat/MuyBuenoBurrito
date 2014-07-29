using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {
	public float moveSpeed = 2f;

	void Start(){
	}

	void Update () {
		if(transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect * 2f){
			Destroy(gameObject);
		}

		transform.position += -transform.right * Time.deltaTime * moveSpeed;
	}
}