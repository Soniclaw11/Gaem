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
    private GameObject crosshair;
    private Sprite crosshairSprite;
    private SpriteRenderer crosshairRenderer;
    public Texture2D crosshairTexture;

    // Use this for initialization
    void Start()
    {
        bulletSpeed = 300000.0f;
        canShoot = true;
        moveHorizontal = 0.0f;
        moveVertical = 0.0f;
        speedValue = 250 * Time.deltaTime;
        rb = gameObject.GetComponent<Rigidbody2D>();

        crosshair = new GameObject("Crosshair");
        crosshairSprite = Sprite.Create(crosshairTexture, new Rect(0.0f, 0.0f, crosshairTexture.width, crosshairTexture.height), new Vector2(0.5f, 0.5f), 65.0f);
        crosshairRenderer = crosshair.AddComponent<SpriteRenderer>();
        crosshairRenderer.sprite = crosshairSprite;
    }

    private void FixedUpdate()
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
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate character during key presses
        if (Input.GetKey(KeyCode.M))
            transform.Rotate(Vector3.forward * -4);

        if (Input.GetKey(KeyCode.N))
            transform.Rotate(Vector3.forward * 4);

        // Crosshair positioning: Raycast ahead the distance the bullet is 100% accurate to the crosshair (see if/else below)
        int layerMask = 1 << 2;
        layerMask = ~layerMask;  // Inverts bitmask so that the ray collides against everything not in layer 2 (bullets are in layer 2)
        RaycastHit2D raycastInfo = Physics2D.Raycast(transform.position + transform.right * 0.2f + transform.up * 0.7f, transform.rotation * Vector2.up, 13.0f, layerMask);
        if (raycastInfo.collider)  // if the ray hit a collider, place crosshair at the place the ray collided
        {
            crosshair.transform.position = raycastInfo.point;
        }
        else  //if it didn't, place the crosshair at the end of the ray
        {
            Ray2D ray = new Ray2D(transform.position + transform.right * 0.2f + transform.up * 0.7f, transform.rotation * Vector2.up);
            crosshair.transform.position = ray.GetPoint(13.0f);
        }
        crosshair.transform.rotation = transform.rotation;

        // Shoot bullets when key pressed. Sets canShoot to false and then starts a one-second timer to set it to true again
        if (Input.GetKeyDown(KeyCode.Comma) && canShoot)
        {
            Rigidbody2D bullet;
            // About the bullet position; we change the spawn point of the bullet from the center of the character to the tip of the gun
            bullet = Instantiate(bulletPrefab, transform.position + transform.right * 0.2f + transform.up * 0.7f, transform.rotation);
            bullet.AddForce(transform.rotation * Vector2.up * bulletSpeed * Time.deltaTime);
            bullet.gameObject.layer = 2;  // move bullet to a separate layer so that the crosshair raycast does not run into it

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
