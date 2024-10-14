using UnityEngine;

public class EffectDisable : MonoBehaviour
{
    public GameObject parent;

    public void OnEnable()
    {
        if(parent.transform.parent != ParController.instance.container)
        {
            parent.transform.SetParent(ParController.instance.container);
            parent.SetActive(false);
        }
    }
}
