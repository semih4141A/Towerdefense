using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 5f;
    public float fireRate = 1f;
    public GameObject homingBulletPrefab;
    public Transform firePoint;

    private float fireCountdown = 0f;
    private Transform target;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;
        FindTarget();

        if (target == null) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Shoot()
    {
        GameObject bulletGO = Instantiate(homingBulletPrefab, firePoint.position, Quaternion.identity);
        HomingBullet bullet = bulletGO.GetComponent<HomingBullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}