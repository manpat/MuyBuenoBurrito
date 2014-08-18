using UnityEngine;
using System.Collections;

public class ParallaxScroll : MonoBehaviour {
	[SerializeField] private Vector2 scrollRate = new Vector2(0.1f, 0.05f); 
	[SerializeField] private bool autoScroll = false; 
	[SerializeField] private float yClampMax = 0.4f; 
	[SerializeField] private float yClampMin = -0.1f; 

	Vector3 pos = new Vector3(0f, 0f);

	void Update(){
		if(autoScroll){
			pos.x += scrollRate.x * Time.deltaTime;
		}else{
			pos.x = transform.position.x/Camera.main.orthographicSize *scrollRate.x;
			pos.y = transform.position.y/Camera.main.orthographicSize *scrollRate.y;
		}


		if(pos.x > 1f){
			pos.x -= 1f;
		}else if(pos.x < 0f){
			pos.x += 1f;
		}

		if(pos.y > yClampMax){
			pos.y = yClampMax;
		}else if(pos.y < yClampMin){
			pos.y = yClampMin;
		}

		renderer.material.mainTextureOffset = pos;
	}
}
