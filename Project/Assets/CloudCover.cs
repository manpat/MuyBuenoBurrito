using UnityEngine;
using System.Collections;

public class CloudCover : MonoBehaviour {
	[SerializeField] private GameObject cloudPrefab;
	[SerializeField] private float cloudDensity = 0.5f;

	void Start () {
		Transform t = transform; 
		Vector3 s = t.localScale;
		Vector3 p = t.position;
		
		int numClouds = (int)Mathf.Ceil(s.x*s.y*cloudDensity);
		for(int i = 0; i < numClouds; ++i){
			Vector3 rp = new Vector3(Random.value-0.5f, Random.value-0.5f, 0f);
			rp = Vector3.Scale(rp, s);
			((GameObject)Instantiate(cloudPrefab, rp + p, Quaternion.identity)).transform.parent = t;
		}
	}

	void OnDrawGizmos(){
		Vector3 p = transform.position;
		Vector3 s = transform.localScale;
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(p, s);
	}
}
