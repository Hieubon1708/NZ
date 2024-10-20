using UnityEngine;

public class EnemyVisible : MonoBehaviour
{
    public GameObject parent;

    private void OnBecameVisible()
    {
        GameController.instance.listEVisible.Add(parent);
    }
}
