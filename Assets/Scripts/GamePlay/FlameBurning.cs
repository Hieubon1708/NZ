using UnityEngine;

public class FlameBurning : MonoBehaviour
{
    public void OnDisable()
    {
        gameObject.SetActive(false);
        transform.SetParent(ParController.instance.container);
    }
}
