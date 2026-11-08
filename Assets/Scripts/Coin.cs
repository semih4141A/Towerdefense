using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin collected!");
            player.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
