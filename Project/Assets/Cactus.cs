using UnityEngine;
using System.Collections;

public class Cactus : Enemy {
	protected override void Start () {
		base.Start();
		deathTime = 0f;
	}
}
