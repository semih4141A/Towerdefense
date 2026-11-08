using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    private Health health;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
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

            health.OnDeath += HandleBaseDestroyed;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {

            health.OnDeath -= HandleBaseDestroyed;
        }
    }



    private void HandleBaseDestroyed()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }
        Debug.Log("<color=red>GAME OVER! Ana Kale Yıkıldı!</color>");
    }
}