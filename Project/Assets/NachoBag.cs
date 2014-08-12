using UnityEngine;
using System.Collections;

public class NachoBag : PickupBase {
    public override void ActivatePickup(){
        print("NachoGot");
        Game.main.player.shurikensRemaining += 1;
        Game.main.player.CreateParticles(new Color(0.8f, 0.1f, 0.2f, 1f));
    }
    
//    public override void DeactivatePickup(){
//        print ("");
//        Game.main.player.attackMultiplier = 1f;
//    }
}
