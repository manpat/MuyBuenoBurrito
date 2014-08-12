using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGameScreen : MonoBehaviour {
	public Font font;
	public Color color;

	TextMesh[] scoreList;
	TextMesh[] nameList;
	List<HighscoreData> hd;
	HighscoreData playerHighscore = new HighscoreData();

	int posInHighscores = -1;
	
	void Start(){
		hd = HighscoreManager.LoadHighscoreData();

		playerHighscore.score = Game.main.CalcScore();
		playerHighscore.name = "";

		hd.Add(playerHighscore);
		HighscoreManager.SortAndClamp(hd);
		posInHighscores = hd.FindIndex(x => (x == playerHighscore));

		scoreList = new TextMesh[Mathf.Min(hd.Count, 10)];
		nameList = new TextMesh[Mathf.Min(hd.Count, 10)];

		float height = Camera.main.orthographicSize*2f - 1f;
		float halfHeight = height/2f;

		float decrement = height/10f;
		float y = halfHeight;

		Vector3 pos = transform.position;

		for(int i = 0; i < hd.Count; ++i){
			GameObject obj = new GameObject("Highscore_name "+i.ToString());

			MeshRenderer mr = obj.AddComponent<MeshRenderer>();
			mr.material = font.material;
			mr.material.color = color;

			nameList[i] = obj.AddComponent<TextMesh>();
			nameList[i].text = hd[i].name;
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
			scoreList[i].text = Mathf.Ceil(hd[i].score).ToString();
			scoreList[i].font = font;
			scoreList[i].fontSize = 150;
			scoreList[i].characterSize = 0.05f;

			scoreList[i].anchor = TextAnchor.MiddleRight;
			obj.transform.parent = transform;
			obj.transform.position = pos + Vector3.up * y + Vector3.right*Camera.main.orthographicSize*1.5f; 

			y -= decrement;
		}

		Destroy(Game.main.gameObject);
	}

	void Update(){
		if(posInHighscores == -1) return;

		nameList[posInHighscores].text = "<color=\"red\">"+playerHighscore.name + "</color> ";
	}

	void OnGUI(){
		Event e = Event.current;
		if (e.type == EventType.KeyDown){
			if (e.keyCode == KeyCode.Backspace && playerHighscore.name.Length > 0){
				playerHighscore.name = playerHighscore.name.Substring(0, playerHighscore.name.Length-1);

			}else if (e.keyCode == KeyCode.Return && (posInHighscores == -1 || playerHighscore.name.Length > 0)){
				HighscoreManager.SaveHighscoreData(playerHighscore);
				Application.LoadLevel("menu");

			}else if((int)e.character > 10 && e.character != ',' && playerHighscore.name.Length < 25){ // Not return
				playerHighscore.name += e.character;
				print((int)e.character);
			}
		}
	}
}
