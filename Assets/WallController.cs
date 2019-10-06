using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("EnemyProjectile")) {
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("PlayerProjectile")) {
            Object.Destroy(collision.gameObject);
        }
    }
}