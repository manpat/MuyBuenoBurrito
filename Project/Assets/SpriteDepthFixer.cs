using UnityEngine;
using System.Collections;

public class SpriteDepthFixer : MonoBehaviour {
	void Start () {
		renderer.sortingOrder = -Mathf.FloorToInt(transform.position.z*2f);

		float grey = 1f - transform.position.z/11f;
		renderer.material.color = new Color(grey, grey, grey);
	}
}
