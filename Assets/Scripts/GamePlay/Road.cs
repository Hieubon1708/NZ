using UnityEngine;

public class Road : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MachineGun"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
