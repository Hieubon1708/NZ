using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClicker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float localScale;
    public GameObject currentObjectSelected;

    public void Start()
    {
        localScale = transform.localScale.z;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        currentObjectSelected = eventData.pointerCurrentRaycast.gameObject;
        ScaleButton(localScale * 0.95f, 0.05f);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ScaleButton(localScale, 0.05f);
        BlockController.instance.CheckButtonStateAll();
    }

    public void ScaleButton(float value, float duration)
    {
        transform.DOKill();
        transform.DOScale(value, duration).SetEase(Ease.Linear);
    }
}
