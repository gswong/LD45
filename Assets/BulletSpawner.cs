using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{   
    [SerializeField]
    private GameObject pooledBullet = null;

    [Tooltip("Maximum number of bullets")]
    [SerializeField]
    private int maximumBulletCount = 1024;

    [Tooltip("Initial bullet rotation angle")]
    [SerializeField]
    private float initialRotation = 0f;

    [Tooltip("Amount of bullet sets")]
    [SerializeField]
    private int bulletSetAmount = 1;

    [Tooltip("Angle between each bullet set")]
    [SerializeField]
    private float bulletSetSpread = 0f;

    [Tooltip("Amount of bullet lines in a set")]
    [SerializeField]
    private int bulletLineAmount = 4;

    [Tooltip("Angle between each bullet line")]
    [SerializeField]
    private float bulletLineSpread = 90f;

    [Tooltip("Bullet speed")]
    [SerializeField]
    private float bulletSpeed = 1;

    [Tooltip("Bullet spawn rate per second")]
    [SerializeField]
    private float bulletSpawnRatePerSecond = 1;

    [Tooltip("Fire spawn rotation rate in degrees per second")]
    [SerializeField]
    private float bulletRotationRate = 90;

    private List<GameObject> bullets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0f, (1 / bulletSpawnRatePerSecond));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, bulletRotationRate * Time.deltaTime);
    }

    private void Fire()
    {
        float currentBulletSetSpread = initialRotation;

        for(int bulletSet = 0; bulletSet < bulletSetAmount; bulletSet++)
        {
            float currentAngle = currentBulletSetSpread;

            for (int bulletLine = 0 ; bulletLine < bulletLineAmount; bulletLine++)
            {
                float bulletDirectionX = transform.position.x + Mathf.Sin((currentAngle * Mathf.PI) / 180f);
                float bulletDirectionY = transform.position.y + Mathf.Cos((currentAngle * Mathf.PI) / 180f);

                Vector3 bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
                Vector2 bulletDirection = (bulletMoveVector - transform.position).normalized * bulletSpeed;

                GameObject bullet = GetBullet();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<Bullet>().SetMoveDirection(bulletDirection);
                bullet.SetActive(true);

                currentAngle = (currentAngle + bulletLineSpread) % 360;
            }

            currentBulletSetSpread = (currentBulletSetSpread + bulletSetSpread) % 360;
        }
    }

    public GameObject GetBullet()
    {
        // return any non-active bullet in pool
        foreach (var bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        // if all bullets in pool are active, try to instantiate more bullets
        if (bullets.Count < maximumBulletCount)
        {
            GameObject newBullet = Instantiate(pooledBullet);
            newBullet.SetActive(false);
            bullets.Add(newBullet);
            return newBullet;
        }

        return null;
    }
}
