using UnityEngine;

public class WeaponVisible : MonoBehaviour
{
    public GameObject parent;

    private void OnBecameInvisible()
    {
        parent.SetActive(false);
    }
}
