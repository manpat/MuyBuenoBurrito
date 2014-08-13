using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	static private PlayerUI main;

	[SerializeField] private Shader shader;
	[SerializeField] private int vignetteSize;
	[SerializeField] private float vignetteMaxAlpha = 0.2f;

	private Texture2D vignette_tex;
	private GameObject vignette_quad;

	void Awake(){
		main = this;
	}

	void Start () {
		int vignetteSize = 15;
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
		vignette_quad.renderer.material.shader = shader;
		vignette_quad.transform.position = Camera.main.transform.position + Vector3.forward * 2f;
		vignette_quad.transform.parent = Camera.main.transform;

		Vector3 scale = new Vector3();

		scale.x = Camera.main.orthographicSize * Camera.main.aspect;
		scale.y = Camera.main.orthographicSize;
		vignette_quad.transform.localScale = scale * 2f;

		SetVignetteIntensity(0f);
		SetVignetteColor(Color.white);
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
}
