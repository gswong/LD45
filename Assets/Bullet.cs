using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 moveDirection;

    private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if(!GetComponent<Renderer>().isVisible)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Invoke("Destroy", 3f);
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

    private void Destroy()
    {
        gameObject.SetActive(false);
    }
}
