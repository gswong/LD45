using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [SerializeField]
    private GameObject pooledBullet = null;

    private bool notEnoughBulletsInPool = true;

    private List<GameObject> bullets = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
        if (notEnoughBulletsInPool)
        {
            GameObject newBullet = Instantiate(pooledBullet);
            newBullet.SetActive(false);
            bullets.Add(newBullet);
            return newBullet;
        }

        return null;
    }
}
