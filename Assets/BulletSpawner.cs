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

    [SerializeField]
    private bool bulletRotationInverted = false;

    public float lifeDuration = 3f;

    private List<GameObject> bullets = new List<GameObject>();

    private float bulletSpawnDeltaThreshold = 0f;

    private float currentRotation = 0f;

    float bulletSpawnDeltaTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = initialRotation;
        bulletSpawnDeltaThreshold = (1 / bulletSpawnRatePerSecond);
    }

    // Update is called once per frame
    void Update()
    {
        bulletSpawnDeltaTime += Time.deltaTime;
        currentRotation = (currentRotation + (bulletRotationRate * Time.deltaTime)) % 360;

        if (bulletSpawnDeltaTime > bulletSpawnDeltaThreshold)
        {
            bulletSpawnDeltaTime = 0;
            Fire();
        }
    }

    private void Fire()
    {
        float currentBulletSetSpread = bulletRotationInverted ? 360 - currentRotation : currentRotation;

        for (int bulletSet = 0; bulletSet < bulletSetAmount; bulletSet++)
        {
            float currentAngle = currentBulletSetSpread;

            for (int bulletLine = 0 ; bulletLine < bulletLineAmount; bulletLine++)
            {
                float bulletDirectionX = transform.position.x + Mathf.Sin((currentAngle * Mathf.PI) / 180f);
                float bulletDirectionY = transform.position.y + Mathf.Cos((currentAngle * Mathf.PI) / 180f);

                Vector3 bulletMoveVector = new Vector3(bulletDirectionX, bulletDirectionY, 0f);
                Vector2 bulletDirection = (bulletMoveVector - transform.position).normalized;

                GameObject bullet = GetBullet();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.GetComponent<Bullet>().SetLifeDuration(lifeDuration);
                bullet.GetComponent<Bullet>().SetMoveSpeed(bulletSpeed);
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
