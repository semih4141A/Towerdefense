using UnityEngine;

public class BuildSpot : MonoBehaviour
{
    [Header("Build Spot Settings")]
    public GameObject towerPrefab;
    public int towercost = 3;

    private bool isPlayerinside = false;

    private PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController p = collision.GetComponent<PlayerController>();
        if (p != null)
        {
            isPlayerinside = true;
            player = p;
            Debug.Log("Player entered build spot");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController p = collision.GetComponent<PlayerController>();
        if (p != null)
        {
            isPlayerinside = false;
            player = null;
            Debug.Log("Player exited build spot");
        }
    }

    private void Update()
    {
        if (isPlayerinside && Input.GetKeyDown(KeyCode.E))
        {
            BuildTower();
        }
    }

    private void BuildTower()
    {
        if (player != null && player.coins >= towercost)
        {
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            player.AddCoin(-towercost);
            Debug.Log("Tower built! Remaining coins: " + player.coins);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not enough coins to build tower!");
        }
    }


}
