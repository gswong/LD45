using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    public float ProjectileLife = 5;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + ProjectileLife) {
            Object.Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // TODO decrement enemy life

        if (collision.gameObject.name == "Tilemap") {
            Object.Destroy(this.gameObject);
        }
    }
}
