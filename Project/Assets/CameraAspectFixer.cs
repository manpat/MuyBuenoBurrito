using UnityEngine;
using System.Collections;

public class CameraAspectFixer : MonoBehaviour {
	void Start () {
		float aspect = Camera.main.aspect;
		float size = Camera.main.orthographicSize;
		float newSize = size*(16.0f/9.0f)/aspect;
		Camera.main.orthographicSize = newSize;
	}
}
