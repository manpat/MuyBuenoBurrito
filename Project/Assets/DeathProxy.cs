using UnityEngine;
using System.Collections;

public class DeathProxy : MonoBehaviour {
	public float deathTime = 5f;
	private Timer timer;

	void Awake () {
		timer = gameObject.AddComponent<Timer>();
	}
	
	void Update () {
		if(timer > deathTime) Destroy(gameObject);
	}
}
