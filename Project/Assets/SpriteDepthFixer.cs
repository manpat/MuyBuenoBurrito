using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class SpriteDepthFixer : MonoBehaviour {

	void Start () {
		renderer.sortingOrder = -Mathf.FloorToInt(transform.position.z*2f);
	}
	
	void Update () {
		if(!EditorApplication.isPlaying){
			renderer.sortingOrder = -Mathf.FloorToInt(transform.position.z*2f);
		}
	}
}
