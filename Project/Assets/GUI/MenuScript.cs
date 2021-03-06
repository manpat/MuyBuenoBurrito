﻿using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	static public MenuScript main;

	public MenuScreen[] screens;
	public int selectedScreen;

	private Vector3 initPos;
	private Timer slideTimer;

	public AudioClip onButtonClick;

	void Awake () {
		main = this;
	}

	void Start(){
		slideTimer = gameObject.AddComponent<Timer>();
		SetSelectedScreen(0);
	}
	
	void Update () {
		Transform t = screens[selectedScreen].transform;
		Vector3 dir = (t.position - t.forward*10f) - initPos;

		Camera.main.transform.position = initPos + dir.normalized * Mathf.SmoothStep(0, dir.magnitude, slideTimer);
	}

	public void DoThing(string action){
		if(onButtonClick) AudioSource.PlayClipAtPoint(onButtonClick, transform.position, 0.5f);

		switch(action){
			case "StartGame":
				Application.LoadLevel("level1");
				break;

			case "MainScreen":
				SetSelectedScreen(0);
				break;

			case "Instructions1":
				SetSelectedScreen(1);
				break;

			case "Instructions2":
				SetSelectedScreen(2);
				break;

			case "Instructions3":
				SetSelectedScreen(3);
				break;

			case "Highscores":
				SetSelectedScreen(4);
				break;

			case "Credits":
				SetSelectedScreen(5);
				break;

			case "Instructions4":
				SetSelectedScreen(6);
				break;

			case "Quit":
				Application.Quit();
				break;

			default:
			break;
		}
	}

	public void SetSelectedScreen(int screen){
		selectedScreen = screen;
		initPos = Camera.main.transform.position;
		slideTimer.Reset();
	}
}
