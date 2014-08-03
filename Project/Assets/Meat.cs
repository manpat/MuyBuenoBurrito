using UnityEngine;
using System.Collections;

public class Meat : MonoBehaviour {

    public bool meatGot = false;
    // time effect lasts, open for changes
    public float effectTime = 5f;
    public float counter;
    
    void Update()
    {
        // has the chili been picked up?
        if (meatGot == true)
        {
            //begin counter
            counter += Time.deltaTime;
            if(counter >= effectTime)
            {
                print ("Outta Meat");
                Game.main.player.attackMultiplier = 1f;
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
            print("MeatGot");
            meatGot = true;
            collider2D.enabled = false;
            renderer.enabled = false;
            Game.main.player.attackMultiplier = 2f;
            Game.main.player.CreateParticles(new Color(0.8f, 0.1f, 0.2f, 1f));
        }
    }
}
