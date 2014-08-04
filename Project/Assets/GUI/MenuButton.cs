using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	public string action = "";

	void OnMouseDown(){
		// Vector2 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Vector2 extent = transform.localScale*0.5f;
		// Vector2 dist = ((Vector2)transform.position - mpos);
		// dist.x = Mathf.Abs(dist.x);
		// dist.y = Mathf.Abs(dist.y);

		// if(dist.x < extent.x && dist.y < extent.y){
			MenuScript.main.DoThing(action);
		// }
	}
}
