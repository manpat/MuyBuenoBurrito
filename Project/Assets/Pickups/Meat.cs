using UnityEngine;
using System.Collections;

public class Meat : PickupBase {
	public override void ActivatePickup(){
		print("MeatGot");
		Game.main.player.attackMultiplier = 2f;
		Game.main.player.CreateParticles(new Color(0.8f, 0.1f, 0.2f, 1f));
		Game.main.player.AddPickupTint(tint);
	}

	public override void DeactivatePickup(){
		print ("Outta Meat");
		Game.main.player.attackMultiplier = 1f;
	}
}
