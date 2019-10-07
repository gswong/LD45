using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{
    public bool allowMovement;
    public float MaxSpeed = 7;
    private float catchLockTime;
    private float hurtInvincibleTime;
    private Vector3 aimProjectile;
    public enum PlayerState {
        CatchReadyNoProjectile,
        CatchingNoProjectile,
        CatchLockedNoProjectile,
        CatchReadyCaughtProjectile,
        HurtInvincible,
        NoLivesRemaining
    }
    public GameObject PlayerProjectile;
    public PlayerState ps;
    private GameObject shield;
    private GameObject cracked;
    private GameObject crosshairs;
    private SpriteRenderer sprite;
    private Color originalColor;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        ps = PlayerState.CatchReadyNoProjectile;
        //rb = GetComponent<Rigidbody2D>();
        shield = GameObject.Find("Shield");
        cracked = GameObject.Find("Cracked");
        crosshairs = GameObject.Find("Crosshairs");
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        allowMovement = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //
        // Movement
        //
        if (allowMovement) {
            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");
            Vector3 tempVect = new Vector3(xAxis, yAxis, 0);
            tempVect = tempVect.normalized * MaxSpeed * Time.deltaTime;
            if (xAxis != 0 || yAxis != 0) {
                aimProjectile = tempVect.normalized;
            }
            if (ps != PlayerState.HurtInvincible && ps != PlayerState.NoLivesRemaining) {
                rb.MovePosition(rb.transform.position + tempVect);
            }
            crosshairs.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        //
        // Spacebar actions
        //
        if ((Input.GetKeyDown("space") || Input.GetMouseButtonDown(0)) && Time.time > catchLockTime && Time.time > hurtInvincibleTime) {
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
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;
                    aimProjectile = (mousePosition - transform.position).normalized;
                    prb.AddForce(aimProjectile * 2 * MaxSpeed);
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
                crosshairs.SetActive(true);
                break;
            case PlayerState.NoLivesRemaining:
                cracked.SetActive(true);
                transform.Rotate(new Vector3(0, 0, 1), -400 * Time.deltaTime);
                break;
            default:
                cracked.SetActive(false);
                shield.SetActive(false);
                crosshairs.SetActive(false);
                transform.rotation = Quaternion.identity;
                break;
        }
        switch (ps) {
            case PlayerState.CatchReadyCaughtProjectile:
                shield.SetActive(true);
                sprite.color = Color.cyan;
                break;
            case PlayerState.CatchingNoProjectile:
                sprite.color = Color.Lerp(originalColor, Color.cyan, Mathf.PingPong(catchLockTime - Time.time, 1));
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
                    Vector3 direction = collision.gameObject.transform.position - transform.position;
                }
                if (collision.gameObject.CompareTag("Enemy")) {
                    ps = PlayerState.HurtInvincible;
                    hurtInvincibleTime = Time.time + 1;
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
    }
}
