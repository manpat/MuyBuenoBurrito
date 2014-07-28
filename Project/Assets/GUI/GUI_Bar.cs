using UnityEngine;
using System.Collections;

public class GUI_Bar : MonoBehaviour {
	public static Texture2D tex;

	public static void DrawBar(int x, int y, int width, int height, float value, Color color){
		if(!tex){
			// Hacky but whatever
			tex = new Texture2D(1,3);
			tex.SetPixel(0, 0, Color.white);
			tex.SetPixel(0, 1, new Color(0.7f, 0.7f, 0.7f));
			tex.SetPixel(0, 2, Color.white);
			tex.Apply();
		}

		value = Mathf.Clamp(value, 0f, 1f);

		GUI.BeginGroup(new Rect(x, y, width, height));
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0, 0, width, height), tex);

			GUI.color = color;
			GUI.DrawTexture(new Rect(0, 0, (int)(width*value), height), tex);
		GUI.EndGroup();
	}
}
