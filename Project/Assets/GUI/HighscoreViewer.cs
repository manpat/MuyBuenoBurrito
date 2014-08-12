using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighscoreViewer : MonoBehaviour {
	public Font font;
	public Color color;

	TextMesh[] scoreList;

	void Start () {
		List<HighscoreData> data = HighscoreManager.LoadHighscoreData();
		scoreList = new TextMesh[Mathf.Min(data.Count, 10)];

		float height = Camera.main.orthographicSize*2f - 1f;
		float halfHeight = height/2f;

		float decrement = height/10f;
		float y = halfHeight;

		Vector3 pos = transform.position;

		for(int i = 0; i < data.Count; ++i){
			GameObject obj = new GameObject("Highscore "+i.ToString());

			MeshRenderer mr = obj.AddComponent<MeshRenderer>();
			mr.material = font.material;
			mr.material.color = color;

			scoreList[i] = obj.AddComponent<TextMesh>();
			scoreList[i].text = BuildString(data[i]);
			scoreList[i].font = font;
			scoreList[i].fontSize = 150;
			scoreList[i].characterSize = 0.05f;

			scoreList[i].anchor = TextAnchor.MiddleCenter;
			obj.transform.parent = transform;
			obj.transform.position = pos + Vector3.up * y; 

			y -= decrement;
		}
	}

	public static string BuildString(HighscoreData dat){
		return dat.name + " " + new string('=', 5) + " " + Mathf.Ceil(dat.score).ToString();
	}
}
