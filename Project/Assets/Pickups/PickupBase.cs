﻿using UnityEngine;
using System.Collections;

public class PickupBase : MonoBehaviour {
	public bool pickupGot = false;
	// time effect lasts, open for changes
	public float effectTime = 5f;
	public float counter;
	
	void Update () {
		// has the pickup been picked up?
		if (pickupGot == true)
		{
			//begin counter
			counter += Time.deltaTime;
			if(counter >= effectTime)
			{
				DeactivatePickup();
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
			ActivatePickup();
		}
	}

	public virtual void ActivatePickup(){}
	public virtual void DeactivatePickup(){}
}