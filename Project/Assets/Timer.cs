using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	private float timeRunning;
	private bool running = true;

	// Use this for initialization
	void Start() {
		timeRunning = 0.0f;
	}
	
	// Update is called once per frame
	void Update() {
		if(running)	timeRunning += Time.deltaTime;
	}

	public void Reset(){
		timeRunning = 0.0f;
	}

	public float GetTime(){
		return timeRunning;
	}

	public void Stop(){
		running = false;
	}

	public void Restart(){
		running = true;
	}

	public static implicit operator float(Timer t){
		return t.timeRunning;
	}
}
