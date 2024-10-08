using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScale : MonoBehaviour
{
    public float localScale;

    public void Start()
    {
        localScale = transform.localScale.z;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ScaleButton(localScale * 0.95f, 0.05f);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        ScaleButton(localScale, 0.05f);
    }

    public void ScaleButton(float value, float duration)
    {
        transform.DOKill();
        transform.DOScale(value, duration).SetEase(Ease.Linear);
    }
}
