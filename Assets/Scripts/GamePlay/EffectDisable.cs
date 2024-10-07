using UnityEngine;

public class EffectDisable : MonoBehaviour
{
    public GameObject parent;

    private void OnBecameInvisible()
    {
        if (parent.gameObject.activeSelf)
        {
            parent.transform.SetParent(ParController.instance.container);
            parent.SetActive(false);
        }
    }
}
