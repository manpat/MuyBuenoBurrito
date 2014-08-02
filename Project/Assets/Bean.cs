using UnityEngine;
using System.Collections;

public class Bean : MonoBehaviour {

    public bool beanGot = false;
    // time effect lasts, open for changes
    public float effectTime = 5f;
    public float counter;

    void Update()
    {
        // has the bean been picked up?
        if (beanGot == true)
        {
            //begin counter
            counter += Time.deltaTime;
            if(counter >= effectTime)
            {
                print ("Outta Gas");
                Game.main.player.canTripleJump = false;
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
            print("Bean Got");
            beanGot = true;
            collider2D.enabled = false;
            renderer.enabled = false;
            Game.main.player.canTripleJump = true;
        }
    }
}
