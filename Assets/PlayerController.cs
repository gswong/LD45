using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Externally control the speed
    public float MaxSpeed = 7;

    // Internally computed speed
    private float speed;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        speed = MaxSpeed;
    }

    // Update is called once per frame
    void Update() {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 tempVect = new Vector3(xAxis, yAxis, 0);

        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);
        //transform.Translate(tempVect);
    }

    void OnCollisionEnter2D(Collision2D collision) {

    }
}
