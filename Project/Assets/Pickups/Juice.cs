using UnityEngine;
using System.Collections;

public class Juice : PickupBase {
    public override void ActivatePickup(){
        print("HP UP");
        Game.main.player.health += 10f;
        Game.main.player.CreateParticles(new Color(1f, 0.2f, 0.3f, 1f));
        print(Game.main.player.health);
    }
}
