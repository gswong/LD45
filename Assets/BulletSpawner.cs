using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    private int bulletCount = 10;

    [SerializeField]
    private float startAngle = 0f;
    
    [SerializeField]
    private float endAngle = 360f;

    private Vector2 bulletMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {    
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletCount;
        float angle = startAngle;

        for (int i = 0 ; i < bulletCount; i++)
        {
            float bulletDirectionX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirectionY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
            Vector2 bulletDirection = (bulletMoveVector - transform.position).normalized;

            GameObject bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().SetMoveDirection(bulletDirection);

            angle += angleStep;
        }
    }
}
