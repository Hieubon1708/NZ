using UnityEngine;

public class EnemyTowerStoper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColStopTower"))
        {
            CarController.instance.multiplier = 0;
        }
    }
}
