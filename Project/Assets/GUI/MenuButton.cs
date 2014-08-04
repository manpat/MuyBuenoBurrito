using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {
	public string action = "";

	void OnMouseDown(){
		MenuScript.main.DoThing(action);
	}
}
