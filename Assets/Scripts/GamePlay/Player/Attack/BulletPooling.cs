using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 50;

    private List<BulletController> bullets = new List<BulletController>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab, transform);
            obj.SetActive(false);

            BulletController bullet = obj.GetComponent<BulletController>();
            bullets.Add(bullet);
        }
    }

    public BulletController Get()
    {

        for (int i = 0; i < bullets.Count; i++)
        {
            //Debug.Log($"Bullet {i} active: {bullets[i].gameObject.activeInHierarchy}");

            if (!bullets[i].gameObject.activeInHierarchy)
            {
               // Debug.Log("Bullet acquired");
                return bullets[i];
            }
        }

        return null; // hard cap
    }
}