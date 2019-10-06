using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 moveDirection;

    private float moveSpeed = 1f;

    private float lifeDuration = 3f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<Renderer>().isVisible)
        {
            gameObject.SetActive(false);
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("Destroy", lifeDuration);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetLifeDuration(float duration)
    {
        lifeDuration = duration;
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision) {
         if (collision.gameObject.name == "Tilemap") {
            this.gameObject.SetActive(false);
        }
    }
}
