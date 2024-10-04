using UnityEngine;

public class UpgradeEvolution : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject panelUpgradeEvo;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIEffect.instance.FadeAll(canvasGroup, 1, 0.25f);
        }
    }
}
