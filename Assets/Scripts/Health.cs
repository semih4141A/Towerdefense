using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Visual Effects")]
    public bool flashOnDamage = true;
    public float flashDuration = 0.1f;

    public event Action OnTakeDamage;
    public event Action OnDeath;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFlashing = false;

    private void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnTakeDamage?.Invoke();

        if (flashOnDamage && spriteRenderer != null && !isFlashing)
        {
            StartCoroutine(FlashRoutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRoutine()
    {
        isFlashing = true;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        isFlashing = false;
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}