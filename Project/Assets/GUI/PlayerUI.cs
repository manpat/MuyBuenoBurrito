using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour {
	static private PlayerUI main;

	[SerializeField] private Shader vignetteShader;
	[SerializeField] private Shader fadeShader;
	[SerializeField] private int vignetteSize = 15;
	[SerializeField] private float vignetteMaxAlpha = 0.2f;

	[SerializeField] private Sprite healthBarActiveSprite;
	[SerializeField] private Sprite healthBarInactiveSprite;
	[SerializeField] private Sprite shurikenSprite;
	[SerializeField] private Font scoreFont;
	[SerializeField] private Color scoreColor;

	private Texture2D fade_tex;
	private Texture2D vignette_tex;
	private GameObject vignette_quad;

	private float healthbarValue = 1f;
	private int numShurikens = 3;
	private float fadeVal = 0f;

	private List<PickupBase> pickups = new List<PickupBase>();

	private bool guiEnabled = true;

	void Awake(){
		main = this;
	}

	void Start() {
		vignette_tex = new Texture2D(vignetteSize, vignetteSize);

		float mag = Mathf.Sqrt(2f*vignetteSize*vignetteSize)/2f;

		for(int y = 0; y < vignetteSize; ++y){
			for(int x = 0; x < vignetteSize; ++x){
				Color col = new Color(1f,1f,1f,1f);

				float center = (float)vignetteSize/2f;
				float dx = (center - x);
				float dy = (center - y);

				dx = dx/mag;
				dy = dy/mag;

				col.a = Mathf.Sqrt(dx*dx + dy*dy);

				vignette_tex.SetPixel(x, y, col);
			}
		}

		vignette_tex.Apply();

		vignette_quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		vignette_quad.renderer.material.mainTexture = vignette_tex;
		vignette_quad.renderer.material.shader = vignetteShader;
		vignette_quad.transform.position = Camera.main.transform.position + Vector3.forward * 2f;
		vignette_quad.transform.parent = Camera.main.transform;

		Vector3 scale = new Vector3();

		scale.x = Camera.main.orthographicSize * Camera.main.aspect;
		scale.y = Camera.main.orthographicSize;
		vignette_quad.transform.localScale = scale * 2f;

		SetVignetteIntensity(0f);
		SetVignetteColor(Color.white);


		fade_tex = new Texture2D(1,1);
		fade_tex.SetPixel(0, 0, Color.white);
		fade_tex.Apply();
	}

	void OnLevelWasLoaded(int i){
		if(!vignette_quad) Start();
	}

	void OnGUI(){
		DrawFade();

		if(guiEnabled){
			DrawScore();
			DrawHealthbar();
			DrawShurikens();
			DrawPickupText();
		}
	}

	void DrawFade(){
		Color prev = GUI.color;

		Color c = Color.black;
		c.a = fadeVal;
		GUI.color = c;

		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fade_tex);

		GUI.color = prev;
	}

	void DrawScore(){
		Rect rect = new Rect();
		rect.width = 200;
		rect.height = 200;
		rect.x = 10;
		rect.y = 5;

		GUI.skin.font = scoreFont;
		GUIStyle style = new GUIStyle();
		style.fontSize = 50;
		style.normal.textColor = scoreColor;

		GUI.contentColor = Color.white;
		GUI.Label(rect, Mathf.FloorToInt(Game.main.CalcScore()).ToString(), style);
	}

	void DrawPickupText(){
		Rect rect = new Rect();
		rect.width = 200;
		rect.height = 200;
		rect.x = Screen.width - 210;
		rect.y = 5;

		GUI.skin.font = scoreFont;
		GUIStyle style = new GUIStyle();
		style.fontSize = 50;
		style.normal.textColor = scoreColor;
		style.alignment = TextAnchor.UpperRight;

		foreach(PickupBase p in pickups){
			GUI.contentColor = p.tint;
			GUI.Label(rect, p.GetName(), style);
			rect.y += 40;
		}
	}

	void DrawShurikens(){
		float scale = Screen.height / Camera.main.orthographicSize;

		Bounds bounds = shurikenSprite.bounds;
		Rect rect = new Rect();
		rect.width = bounds.size.x * scale;
		rect.height = bounds.size.y * scale;
		rect.x = Screen.width - rect.width - 10;
		rect.y = Screen.height - rect.height;

		for(int i = 0; i < numShurikens; ++i){
			GUI.DrawTexture(rect, shurikenSprite.texture);
			rect.x -= rect.width+2;
		}
	}

	void DrawHealthbar(){
		float scale = Screen.height / Camera.main.orthographicSize;

		Bounds bounds = healthBarActiveSprite.bounds;
		Rect rect = new Rect();
		rect.width = bounds.size.x * scale;
		rect.height = bounds.size.y * scale;
		rect.x = 20;
		rect.y = Screen.height - rect.height;

		GUI.BeginGroup(rect);
			rect.height *= 2f;
			rect.x = 0;
			rect.y = -bounds.size.y * scale;

			GUI.DrawTexture(rect, healthBarInactiveSprite.texture);

			GUI.BeginGroup(new Rect(0, 0, rect.width*healthbarValue, rect.height));
				rect.y = 0f;
				GUI.DrawTexture(rect, healthBarActiveSprite.texture);
			GUI.EndGroup();
		GUI.EndGroup();
	}

	static public void SetEnabled(bool e){
		main.guiEnabled = e;
		if(!main.guiEnabled){
			SetVignetteIntensity(0f);
		}
	}

	static public void AddPickup(PickupBase p){
		if(!main.pickups.Exists((x) => (x == p))) main.pickups.Add(p);
	}
	static public void RemovePickup(PickupBase p){
		main.pickups.RemoveAll((x) => (x == p));
	}

	static public void OnPlayerDeath(){
		main.pickups.Clear();
	}

	static public void SetVignetteIntensity(float a){
		Color col = main.vignette_quad.renderer.material.GetColor("_TintColor");
		col.a = main.vignetteMaxAlpha*Mathf.Clamp(a, 0f, 1f);
		main.vignette_quad.renderer.material.SetColor ("_TintColor", col);
	}
	static public void SetVignetteColor(Color col){
		float a = main.vignette_quad.renderer.material.GetColor("_TintColor").a;
		col.a = main.vignetteMaxAlpha*Mathf.Clamp(a, 0f, 1f);
		main.vignette_quad.renderer.material.SetColor ("_TintColor", col);
	}

	static public void SetFade(float a){
		main.fadeVal = a;
	}

	static public void SetHealthbarValue(float h){
		main.healthbarValue = Mathf.Clamp(h, 0f, 1f);
	}
	static public void SetNumShurikens(int n){
		main.numShurikens = n;
	}
}
