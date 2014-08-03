using UnityEngine;
using System.Collections;

public class Chili : MonoBehaviour {

	public bool chiliGot = false;
    // time effect lasts, open for changes
    public float effectTime = 5f;
    public float counter;

    void Update()
    {
        // has the chili been picked up?
        if (chiliGot == true)
        {
            //begin counter
            counter += Time.deltaTime;
            if(counter >= effectTime)
            {
                print ("Outta Juice");
                Game.main.player.speedMultiplier = 1f;
                Destroy(gameObject);
            }
        }
    }

    // Collision, check if player. True = go "invis", mod player stats, 
    // when timer is up, return player stats to normal and destroy self
   public void OnCollisionEnter2D(Collision2D col)
    {
        string tag = col.gameObject.tag;
        if (tag == "Player")
        {
            print("Chili Got");
            chiliGot = true;
            collider2D.enabled = false;
            renderer.enabled = false;
            // needs adjusting, double speed seems too fast??
            Game.main.player.speedMultiplier = 2f;
            Game.main.player.CreateParticles(new Color(0.9f, 0.7f, 0.2f, 1f));
        }
    }
}
