using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 localScale;

    public void Start()
    {
        localScale = transform.localScale;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ScaleButton(localScale * 0.95f, 0.05f);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ScaleButton(localScale, 0.05f);
        BlockController.instance.CheckButtonStateAll();
    }

    public void ScaleButton(Vector3 value, float duration)
    {
        transform.DOKill();
        transform.DOScale(value, duration).SetEase(Ease.Linear);
    }
}
