using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    //
    // Externally control the speed
    //
    public float MaxSpeed = 7;
    public float KnockBack = 50;
    public float Lives = 3;
    public GameObject PlayerProjectile;
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
    private Vector3 aimProjectile;
    private Color originalColor;
    private GameObject shield;

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
        shield = GameObject.Find("Shield");
        shield.SetActive(false);
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        speed = MaxSpeed;
        catchLockTime = 0;
        hurtInvincibleTime = 0;
        scale = transform.localScale;
        ps = PlayerState.CatchReadyNoProjectile;
        aimProjectile = new Vector3(1, 0, 0);
        aimProjectile = aimProjectile.normalized;
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
        if (xAxis != 0 || yAxis != 0) {
            aimProjectile = tempVect.normalized;
        }
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
                    ps = PlayerState.CatchingNoProjectile;
                    GameObject projectile = Instantiate(PlayerProjectile, transform.position, Quaternion.identity) as GameObject;
                    Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    Rigidbody2D prb = projectile.GetComponent<Rigidbody2D>();
                    prb.AddForce(aimProjectile * 2 * KnockBack);
                    catchLockTime = Time.time + 0.5f;
                    break;
                default:
                    break;
            }
        }

        //
        // No spacebar actions
        //
        switch (ps) {
            case PlayerState.CatchingNoProjectile:
            case PlayerState.CatchLockedNoProjectile:
                if (Time.time > catchLockTime) {
                    // No input and time expired
                    ps = PlayerState.CatchReadyNoProjectile;
                }
                break;
            case PlayerState.HurtInvincible:
                if (Time.time > hurtInvincibleTime) {
                    ps = PlayerState.CatchReadyNoProjectile;
                }
                break;
            case PlayerState.CatchReadyNoProjectile:
            case PlayerState.CatchReadyCaughtProjectile:
            case PlayerState.NoLivesRemaining:
            default:
                break;
        }

        //
        // Visual changes
        //
        switch (ps) {
            case PlayerState.CatchReadyCaughtProjectile:
                shield.SetActive(true);
                sprite.color = Color.cyan;
                break;
            case PlayerState.CatchingNoProjectile:
                sprite.color = originalColor;
                //Vector3 scaleVect = new Vector3(0.1f, 0.1f, 0);
                //transform.localScale = scale + scaleVect;
                break;
            case PlayerState.HurtInvincible:
                sprite.color = originalColor;
                Color tempColor = sprite.color;
                tempColor.a = 0.5f;
                sprite.color = tempColor;
                break;
            default:
                //transform.localScale = scale;
                shield.SetActive(false);
                sprite.color = originalColor;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Only modify state, timers, and lives
        switch (ps) {
            case PlayerState.CatchReadyNoProjectile:
            case PlayerState.CatchLockedNoProjectile:
                if (collision.gameObject.CompareTag("EnemyProjectile")) {
                    ps = PlayerState.HurtInvincible;
                    collision.gameObject.SetActive(false);
                    hurtInvincibleTime = Time.time + 1;
                    if (Lives >= 0) {
                        Lives--;
                    }
                    Vector3 direction = collision.gameObject.transform.position - transform.position;
                    direction = -direction.normalized;
                    rb.AddForce(direction * KnockBack);
                }
                if (collision.gameObject.CompareTag("Enemy")) {
                    ps = PlayerState.HurtInvincible;
                    hurtInvincibleTime = Time.time + 1;
                    if (Lives >= 0) {
                        Lives--;
                    }
                    Vector3 direction = collision.gameObject.transform.position - transform.position;
                    direction = -direction.normalized;
                    rb.AddForce(direction * KnockBack);
                }
                break;
            case PlayerState.CatchingNoProjectile:
                if (collision.gameObject.CompareTag("EnemyProjectile")) {
                    ps = PlayerState.CatchReadyCaughtProjectile;
                    collision.gameObject.SetActive(false);
                    catchLockTime = 0;
                }
                break;
            case PlayerState.CatchReadyCaughtProjectile:
                if (collision.gameObject.CompareTag("EnemyProjectile")) {
                    ps = PlayerState.CatchReadyNoProjectile;
                    collision.gameObject.SetActive(false);
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
