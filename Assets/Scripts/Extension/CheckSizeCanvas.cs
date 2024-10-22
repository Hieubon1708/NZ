using UnityEngine;
using UnityEngine.UI;

public class CheckSizeCanvas : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private CanvasScaler canvasScaler;

    void Awake()
    {
        CheckSize();
    }

    public void CheckSize()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }

        canvasScaler.matchWidthOrHeight = cam.aspect < 0.55f ? 0 : 1;
    }
}
