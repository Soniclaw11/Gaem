﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private bool hasRebounded;
    private Rigidbody2D rb;
    private Vector3 oldVelocity;

	// Use this for initialization
	void Start ()
    {
        hasRebounded = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void FixedUpdate()
    {
        oldVelocity = rb.velocity;  // store velocity every frame to use in ricochet physics calculations
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
            else  // else, calculate ricochet physics
            {
                // mark that the bullet has now rebounded for the first time
                hasRebounded = true;

                // get the point of contact
                ContactPoint2D contact = collision.contacts[0];  

                // reflect the old velocity off the wall's normal (perpendicular) vector
                Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, contact.normal);

                // assign the reflected velocity to the rigidbody
                rb.velocity = reflectedVelocity;

                // rotate the object by the same amount we changed its velocity
                Quaternion rotation = Quaternion.FromToRotation(oldVelocity, reflectedVelocity);
                transform.rotation = rotation * transform.rotation;
            }
        }
    }
}
