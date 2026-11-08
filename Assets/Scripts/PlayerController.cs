using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Crosshair Shooting")]
    public Transform crosshair;
    public float crosshairDistance = 2f;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Economy")]
    public int coins = 0;


    private Vector2 mouseworldposition;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (GameManager.Instance?.IsGameEnded == true) return;
        mouseworldposition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());


        UpdateCrosshairPosition();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance?.IsGameEnded == true) return;
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);

    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }


    private void OnAttack(InputValue value)
    {
        if (GameManager.Instance?.IsGameEnded == true) return;

        Shoot();

    }

    private void UpdateCrosshairPosition()
    {
        Vector2 aimdirection = (mouseworldposition - rb.position).normalized;
        crosshair.position = rb.position + aimdirection * crosshairDistance;

        float angle = Mathf.Atan2(aimdirection.y, aimdirection.x) * Mathf.Rad2Deg;
        crosshair.rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    private void Shoot()
    {

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

    }

    public void AddCoin(int amount)
    {
        coins += amount;
        Debug.Log("Mevcut Altın: " + coins);
    }

    public void ApplyKnockback(Vector2 attackerPosition, float force)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * force, ForceMode2D.Impulse);
    }
}
