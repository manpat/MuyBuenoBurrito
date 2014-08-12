using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighscoreViewer : MonoBehaviour {
	public Font font;
	public Color color;

	TextMesh[] scoreList;
	TextMesh[] nameList;

	void Start () {
		List<HighscoreData> data = HighscoreManager.LoadHighscoreData();
		nameList = new TextMesh[Mathf.Min(data.Count, 10)];
		scoreList = new TextMesh[Mathf.Min(data.Count, 10)];

		float height = Camera.main.orthographicSize*2f - 1f;
		float halfHeight = height/2f;

		float decrement = height/10f;
		float y = halfHeight;

		Vector3 pos = transform.position;

		for(int i = 0; i < data.Count; ++i){
			GameObject obj = new GameObject("Highscore_name "+i.ToString());

			MeshRenderer mr = obj.AddComponent<MeshRenderer>();
			mr.material = font.material;
			mr.material.color = color;

			nameList[i] = obj.AddComponent<TextMesh>();
			nameList[i].text = data[i].name;
			nameList[i].font = font;
			nameList[i].fontSize = 150;
			nameList[i].characterSize = 0.05f;

			nameList[i].anchor = TextAnchor.MiddleLeft;
			obj.transform.parent = transform;
			obj.transform.position = pos + Vector3.up * y - Vector3.right*Camera.main.orthographicSize*1.5f; 

			obj = new GameObject("Highscore_score "+i.ToString());

			mr = obj.AddComponent<MeshRenderer>();
			mr.material = font.material;
			mr.material.color = color;

			scoreList[i] = obj.AddComponent<TextMesh>();
			scoreList[i].text = Mathf.Ceil(data[i].score).ToString();
			scoreList[i].font = font;
			scoreList[i].fontSize = 150;
			scoreList[i].characterSize = 0.05f;

			scoreList[i].anchor = TextAnchor.MiddleRight;
			obj.transform.parent = transform;
			obj.transform.position = pos + Vector3.up * y + Vector3.right*Camera.main.orthographicSize*1.5f; 

			y -= decrement;
		}
	}
}
