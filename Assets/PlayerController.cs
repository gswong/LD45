using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //
    // Externally control the speed
    //
    public float MaxSpeed = 7;

    public float Lives = 3;

    //
    // Internally computed speed
    //
    private float speed;

    private float catchLockTime;
    private Vector3 scale;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool isCatching;
    private bool caughtProjectile;
    private bool isHurt;
    private float hurtInvincibleTime;

    public enum PlayerState {
        CatchReadyNoProjectile,
        CatchingNoProjectile,
        CatchLockedNoProjectile,
        CatchReadyCaughtProjectile,
        HurtInvincible,
        NoLivesRemaining
    }

    public PlayerState ps;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.white;
        speed = MaxSpeed;
        catchLockTime = 0;
        hurtInvincibleTime = 0;
        scale = transform.localScale;
        ps = PlayerState.CatchReadyNoProjectile;
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
        if (ps != PlayerState.HurtInvincible && ps != PlayerState.NoLivesRemaining) {
            rb.MovePosition(rb.transform.position + tempVect);
        }

        //
        // Spacebar actions
        //
        if (Input.GetKeyDown("space") && Time.time > catchLockTime && Time.time > hurtInvincibleTime) {
            switch (ps) {
                case PlayerState.CatchReadyNoProjectile:
                    ps = PlayerState.CatchingNoProjectile;
                    catchLockTime = Time.time + 0.5f;
                    break;
                case PlayerState.CatchReadyCaughtProjectile:
                    // TODO emit player projectile
                    ps = PlayerState.CatchingNoProjectile;
                    catchLockTime = Time.time + 0.5f;
                    break;
                default:
                    break;
            }
        }
        if (Time.time > catchLockTime) {
            // No input and time expired
            ps = PlayerState.CatchReadyNoProjectile;
        }

        if (Time.time > hurtInvincibleTime) {
            ps = PlayerState.CatchReadyNoProjectile;
        }

        //
        // Visual
        //
        switch (ps) {
            case PlayerState.CatchReadyCaughtProjectile:
                sprite.color = Color.cyan;
                break;
            case PlayerState.CatchingNoProjectile:
                sprite.color = Color.white;
                Vector3 scaleVect = new Vector3(0.1f, 0.1f, 0);
                transform.localScale = scale + scaleVect;
                break;
            case PlayerState.HurtInvincible:
                sprite.color = Color.white;
                Color tempColor = sprite.color;
                tempColor.a = 0.5f;
                sprite.color = tempColor;
                break;
            default:
                transform.localScale = scale;
                sprite.color = Color.white;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        switch (ps) {
            case PlayerState.CatchReadyNoProjectile:
            case PlayerState.CatchLockedNoProjectile:
                if (collision.gameObject.CompareTag("EnemyProjectile")) {
                    ps = PlayerState.HurtInvincible;
                    Destroy(collision.gameObject);
                    hurtInvincibleTime = Time.time + 1;
                    if (Lives >= 0) {
                        Lives--;
                    }
                }
                if (collision.gameObject.CompareTag("Enemy")) {
                    ps = PlayerState.HurtInvincible;
                    hurtInvincibleTime = Time.time + 1;
                    if (Lives >= 0) {
                        Lives--;
                    }
                    Vector3 direction = collision.transform.position - transform.position;
                    direction = -direction.normalized;
                    rb.AddForce(direction * MaxSpeed);
                }
                break;
            case PlayerState.CatchingNoProjectile:
                if (collision.gameObject.CompareTag("EnemyProjectile")) {
                    ps = PlayerState.CatchReadyCaughtProjectile;
                    Destroy(collision.gameObject);
                    catchLockTime = 0;
                }
                break;
            case PlayerState.CatchReadyCaughtProjectile:
                if (collision.gameObject.CompareTag("EnemyProjectile")) {
                    ps = PlayerState.CatchReadyCaughtProjectile;
                }
                if (collision.gameObject.CompareTag("Enemy")) {
                    ps = PlayerState.CatchReadyNoProjectile;
                    // TODO damage the enemy
                }
                break;
            case PlayerState.HurtInvincible:
                break;
            default:
                break;
        }

        if (Lives <= 0) {
            // TODO Game Over
            ps = PlayerState.NoLivesRemaining;
        }
    }
}
