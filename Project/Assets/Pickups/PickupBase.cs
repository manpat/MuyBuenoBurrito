﻿using UnityEngine;
using System.Collections;

public class PickupBase : MonoBehaviour {
	[SerializeField] private AudioClip onActiveSound;
	[SerializeField] private string pickupName;

	public bool pickupGot = false;
	// time effect lasts 5ish secs, open for changes
	public float effectTime = 5f;
	public float counter;

	public Color tint = Color.white;
	
	void Update () {
		// has the pickup been picked up?
		if (pickupGot == true)
		{
			//begin counter
			counter += Time.deltaTime;
			if(counter >= effectTime)
			{
				DeactivatePickup();
				Game.main.player.RemovePickupTint(tint);
				PlayerUI.RemovePickup(this);
				Destroy(gameObject);
			}
		}
	}
	
	// Collision, check if player. True = go "invis", mod player stats, 
	// when timer is up, return player stats to normal and destroy self
	void OnCollisionEnter2D(Collision2D col) {
		string tag = col.gameObject.tag;
		if (tag == "Player")
		{
			pickupGot = true;
			collider2D.enabled = false;
			renderer.enabled = false;
			Game.main.IncStat("PickupsGot");
			if(onActiveSound) AudioSource.PlayClipAtPoint(onActiveSound, transform.position, 1f);
			ActivatePickup();
			PlayerUI.AddPickup(this);
		}
	}

	public string GetName(){
		return pickupName;
	}

	public virtual void ActivatePickup(){}
	public virtual void DeactivatePickup(){}
}
