using UnityEngine;
using System.Collections;

public class Cactus : Enemy {
	private Timer attackTimer;

	protected override void Start () {
		base.Start();
		attackTimer = gameObject.AddComponent<Timer>();
	}

	protected override void Attack(GameObject player){
		if(attackTimer < 1f/attackRate) return;
		
		player.SendMessage("TakeDamage", attack, SendMessageOptions.DontRequireReceiver);
		attackTimer.Reset();
	}
}
