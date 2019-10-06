using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    private Transform player = null;

    public int Health = 1;

    public float moveSpeed = 5f;
    
    public float minDistaceFromPlayer = 5;

    public float maxDistanceFromPlayer = 10;

    //In what time will the enemy complete the journey between its position and the players position
    public float smoothTime = 10.0f;

    //Vector3 used to store the velocity of the enemy
    private Vector3 smoothVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > minDistaceFromPlayer && distance < maxDistanceFromPlayer)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.position, 
                    ref smoothVelocity, smoothTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("PlayerProjectile")) {
            Health--;
        }
        if (Health <= 0) {
            Object.Destroy(this.gameObject);
        }
    }
}