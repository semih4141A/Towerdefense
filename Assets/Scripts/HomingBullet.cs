using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 15f;
    public int damage = 1;
    public float lifetime = 5f;

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, target.position) <= 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        Health enemyHealth = target.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}