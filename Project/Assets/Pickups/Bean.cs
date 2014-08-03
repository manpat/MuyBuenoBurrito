using UnityEngine;
using System.Collections;

public class Bean : PickupBase {
	public override void ActivatePickup(){
		print("Bean Got");
		Game.main.player.canTripleJump = true;
		Game.main.player.CreateParticles(new Color(0.2f, 0.9f, 0.4f, 1f));
	}

	public override void DeactivatePickup(){
		print ("Outta Gas");
		Game.main.player.canTripleJump = false;
	}
}
