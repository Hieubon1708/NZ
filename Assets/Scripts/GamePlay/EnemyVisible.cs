using UnityEngine;

public class EnemyVisible : MonoBehaviour
{

    private void OnBecameVisible()
    {
        GameController.instance.listEVisible.Add(gameObject);
    }

    private void OnBecameInvisible()
    {
        GameController.instance.listEVisible.Remove(gameObject);
    }
}
