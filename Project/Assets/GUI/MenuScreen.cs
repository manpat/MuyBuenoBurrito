using UnityEngine;
using System.Collections;

public class MenuScreen : MonoBehaviour {
	void OnDrawGizmos(){
		Vector3 viewportSize = new Vector3();
		viewportSize.y = Camera.main.orthographicSize*2f;
		viewportSize.x = viewportSize.y * Camera.main.aspect;
		viewportSize.z = 1f;

		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, viewportSize);
	}
}
