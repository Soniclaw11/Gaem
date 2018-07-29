using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speedValue;
    private float moveHorizontal;
    private float moveVertical;
    public Rigidbody2D bulletPrefab;
    private bool canShoot;
    private IEnumerator coroutine;
    private float bulletSpeed;

    // Use this for initialization
    void Start()
    {
        bulletSpeed = 300000.0f;
        canShoot = true;
        moveHorizontal = 0.0f;
        moveVertical = 0.0f;
        speedValue = 250 * Time.deltaTime;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set speed in cardinal directions when keyboard key is pressed
        if (Input.GetKeyDown(KeyCode.UpArrow))
            moveVertical = speedValue;
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
            moveVertical = -speedValue;
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
            moveHorizontal = speedValue;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            moveHorizontal = -speedValue;
        

        // Reset speed to 0 in cardinal directions when keyboard key is lifted
        if (Input.GetKeyUp(KeyCode.UpArrow) && (moveVertical == speedValue))
            moveVertical = 0;
        
        if (Input.GetKeyUp(KeyCode.DownArrow) && (moveVertical == -speedValue))
            moveVertical = 0;
        
        if (Input.GetKeyUp(KeyCode.RightArrow) && (moveHorizontal == speedValue))
            moveHorizontal = 0;
        
        if (Input.GetKeyUp(KeyCode.LeftArrow) && (moveHorizontal == -speedValue))
            moveHorizontal = 0;

        // Access current speed values in each direction and apply them to the object
        rb.velocity = new Vector2(moveHorizontal, moveVertical);

        // Rotate character during key presses
        if (Input.GetKey(KeyCode.M))
            transform.Rotate(Vector3.forward * -4);

        if (Input.GetKey(KeyCode.N))
            transform.Rotate(Vector3.forward * 4);

        // Shoot bullets when key pressed. Sets canShoot to false and then starts a one-second timer to set it to true again
        if (Input.GetKeyDown(KeyCode.Comma) && canShoot)
        {
            Rigidbody2D bullet;
            // About the bullet position; we change the spawn point of the bullet from the center of the character to the tip of the gun
            bullet = Instantiate(bulletPrefab, transform.position + transform.right * 0.2f + transform.up * 0.7f, transform.rotation);
            bullet.AddForce(transform.rotation * Vector2.up * bulletSpeed * Time.deltaTime);

            canShoot = false;
            coroutine = shootTimer(1.0f);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator shootTimer(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        canShoot = true;
    }
}
