using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class SpriteDepthFixer : MonoBehaviour {

	void Start () {
		renderer.sortingOrder = -Mathf.FloorToInt(transform.position.z*2f);

		if(EditorApplication.isPlaying){
			float grey = 1f - transform.position.z/11f;
			renderer.material.color = new Color(grey, grey, grey);
		}
	}
	
	void Update () {
		if(!EditorApplication.isPlaying){
			renderer.sortingOrder = -Mathf.FloorToInt(transform.position.z*2f);
		}
	}
}
