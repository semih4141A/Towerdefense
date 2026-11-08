using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float movespeed = 2f;
    public int coinreward = 1;
    public int damageToBase = 1;

    [Header("Player Attack Settings")]
    public int damageToPlayer = 1;       // Karaktere kaç vuracak
    public float attackCooldown = 1f;    // Kaç saniyede bir vuracak (sıkışırsan sürekli vurmasın)
    private float lastAttackTime;

    private float knockbackForce = 1f;

    [Header("Loot & Visuals")]
    public GameObject coinPrefab;

    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Health health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        if (health != null)
        {

            health.OnDeath += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {

            health.OnDeath -= HandleDeath;
        }
    }

    private void Start()
    {
        GameObject baseObj = GameObject.FindGameObjectWithTag("Base");
        if (baseObj != null)
        {
            target = baseObj.transform;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * movespeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }


    private void HandleDeath()
    {
        Debug.Log($"Enemy died! Reward: {coinreward} coins");

        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. BASE'E DEĞERSE (Base'e hasar verir ve kendini siler)
        if (collision.gameObject.CompareTag("Base"))
        {
            Debug.Log("Enemy reached the base!");

            Health baseHealth = collision.gameObject.GetComponent<Health>();
            if (baseHealth != null)
            {
                baseHealth.TakeDamage(damageToBase);
            }

            Destroy(gameObject);
        }
    }

    // KARAKTERE DEĞDİĞİ SÜRECE (Karakterin canını yakar ama düşman silinmez!)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Belirlediğimiz cooldown süresi dolduysa vur
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Health playerHealth = collision.gameObject.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageToPlayer);
                    Debug.Log($"Karakter vuruldu! Hasar: {damageToPlayer}");
                }
                PlayerController playerMovement = collision.gameObject.GetComponent<PlayerController>();
                if (playerMovement != null)
                {
                    playerMovement.ApplyKnockback(transform.position, knockbackForce);
                }

                lastAttackTime = Time.time;
            }
        }
    }
}