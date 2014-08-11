using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class HighscoreData{
	public string name;
	public float score;
}

public class HighscoreManager : MonoBehaviour {
	public static HighscoreManager main = null;

	void Awake(){
		if(main) {
			Destroy(gameObject);
			return;
		}

		main = this;
		DontDestroyOnLoad(this);
	}

	public static List<HighscoreData> LoadHighscoreData(){
		List<HighscoreData> highscoreData = new List<HighscoreData>();
		string filePath = Application.persistentDataPath + "/highscores.highscores";

		if(!File.Exists(filePath)){
			return highscoreData;
		}

		string[] lines = File.ReadAllLines(filePath);
		foreach(string line in lines){
			// score,name

			HighscoreData hd = new HighscoreData();
			char[] delim = {','};
			string[] vals = line.Split(delim);
			hd.score = float.Parse(vals[0]);
			hd.name = vals[1];

			highscoreData.Add(hd);
		}

		SortAndClamp(highscoreData);

		return highscoreData;
	}

	public static int SaveHighscoreData(HighscoreData _data){
		string filePath = Application.persistentDataPath + "/highscores.highscores";
		int posInHighscores = -1;

		List<HighscoreData> highscoreData = LoadHighscoreData();
		highscoreData.Add(_data);

		SortAndClamp(highscoreData);

		posInHighscores = highscoreData.FindIndex(x => (x == _data));

		string[] serialised = new string[highscoreData.Count];
		for(int i = 0; i < highscoreData.Count; ++i){
			serialised[i] = highscoreData[i].score.ToString() + "," + highscoreData[i].name;
		}

		File.WriteAllLines(filePath, serialised);
		print("Highscores saved at " + filePath);

		return posInHighscores;
	}

	public static void SortAndClamp(List<HighscoreData> l){
		if(l.Count > 1){
			l.Sort((a,b) => (int)Mathf.Sign(b.score - a.score));
		}

		while(l.Count > 10){ // Remove entries after the 10th
			l.RemoveAt(l.Count-1);
		}
	}
}
