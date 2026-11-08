using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        enemy?.TakeDamage(1);


        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

}
