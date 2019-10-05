using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //
    // Externally control the speed
    //
    public float MaxSpeed = 7;

    //
    // Internally computed speed
    //
    private float speed;

    private bool isCatching;
    private float canCatchTime;
    private Vector3 scale;
    private bool caughtProjectile;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
        speed = MaxSpeed;
        isCatching = false;
        canCatchTime = 0;
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        //
        // Movement
        //
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        Vector3 tempVect = new Vector3(xAxis, yAxis, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);

        //
        // Visual
        //
        if (caughtProjectile) {
            sprite.color = Color.cyan;
        }
        else {
            sprite.color = Color.white;
        }

        //
        // Spacebar actions
        //
        if (Input.GetKeyDown("space")) {
            //
            // Throw projectile
            //
            if (caughtProjectile) {
                // TODO shoot projectile outwards with projectile spawner
                caughtProjectile = false;
            }

            //
            // Catch projectile
            //
            if (Time.time > canCatchTime) {                
                isCatching = true;
                Vector3 scaleVect = new Vector3(0.1f, 0.1f, 0);
                transform.localScale += scaleVect;
                // Set lockout timer to prevent spamming
                canCatchTime = Time.time + 0.5f;
            }
        }
        if (Time.time > canCatchTime) {
            // No input and time expired
            isCatching = false;
            transform.localScale = scale;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (isCatching && collision.gameObject.CompareTag("EnemyProjectile")) {
            caughtProjectile = true;
            Destroy(collision.gameObject);
        }
        // TODO if not isCatching and ran into enemy projectile
        if (!isCatching && collision.gameObject.CompareTag("EnemyProjectile")) {
            Destroy(collision.gameObject);
        }
        // TODO 
    }
}
