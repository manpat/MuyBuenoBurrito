using UnityEngine;
using System.Collections;

public class Blamo : MonoBehaviour {
	public float lifeTime = 2f;
	public float explodeTime = 0.25f;
	public float explodeScale = 0.25f;

	private Timer timer;

	void Start () {
		timer = gameObject.AddComponent<Timer>();
		transform.localScale = Vector3.zero;		
	}
	
	void Update () {
		transform.localScale = Vector3.one * TweenFunction(0f, explodeScale, timer, explodeTime);

		if(timer > 2f*lifeTime/3f) {
			Color c = renderer.material.color;
			c.a = LinearTweenFunction(1f, 0f, timer-2f*lifeTime/3f, lifeTime/3f);
			renderer.material.color = c;
		}
		if(timer > lifeTime) Destroy(gameObject);
	}

	float LinearTweenFunction(float from, float to, float t, float time){
		t = Mathf.Clamp(t/time, 0f, 1f);
		return from*(1f-t) + to*t;
	}

	float TweenFunction(float from, float to, float t, float time){
		t = Mathf.Clamp(t/time, 0f, 1f);
		float a = t*t;
		return from*(1f-a) + to*a;
	}
}
