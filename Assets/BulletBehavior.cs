using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    bool hasRebounded;

	// Use this for initialization
	void Start ()
    {
        hasRebounded = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            // Add knockback to player in direction bullet was traveling at a force of 20 times the velocity
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(gameObject.GetComponent<Rigidbody2D>().velocity * 20);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            if (hasRebounded)  // if already ricocheted once before colliding with this wall, kill bullet
            {
                Destroy(gameObject);
            }
            else
            {
                // mark that the bullet has now rebounded once
                hasRebounded = true;

                // move on to calculate ricochet physics

                // get point of contact with wall and bullet
                ContactPoint2D contact = collision.contacts[0];
            }
        }
    }
}
