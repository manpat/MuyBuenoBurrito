using UnityEngine;
using System.Collections;

public class Chili : PickupBase {
	public override void ActivatePickup(){
		print("Chili Got");
		Game.main.player.speedMultiplier = 2f;
		Game.main.player.CreateParticles(new Color(0.9f, 0.7f, 0.2f, 1f));
		Game.main.player.AddPickupTint(tint);
	}

	public override void DeactivatePickup(){
		print ("Outta Juice");
		Game.main.player.speedMultiplier = 1f;
	}
}
