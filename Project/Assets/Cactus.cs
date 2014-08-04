using UnityEngine;
using System.Collections;

public class Cactus : Enemy {
	public GameObject cactusShardPrefab;

	protected override void Start () {
		base.Start();
		deathTime = 0f;
	}

	protected override void OnDie(){
		print("Cactus dead");
	}
}
