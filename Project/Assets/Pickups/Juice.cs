using UnityEngine;
using System.Collections;

public class Juice : PickupBase {
	public float restoreAmount = 30f;

	public override void ActivatePickup(){
		Game.main.player.health += restoreAmount;
		Game.main.player.CreateParticles(new Color(1f, 0.2f, 0.3f, 1f));
	}
}
