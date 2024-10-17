using DG.Tweening;
using TMPro;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Transform parent;
    public RectTransform canvas;
    public RectTransform rectDamage;
    public TextMeshProUGUI textDamage;
    public Rigidbody2D rb;
    bool isShowDamage;

    public void HitEffect(SpriteRenderer[] fullBodies, float value)
    {
        for (int i = 0; i < fullBodies.Length; i++)
        {
            fullBodies[i].material.SetFloat("_HitEffectBlend", value);
        }
    }

    public void ShowDamage(string text, GameObject content, SpriteRenderer[] fullBodies)
    {
        if (isShowDamage) return;
        canvas.SetParent(GameController.instance.poolDamages);
        textDamage.text = text;
        isShowDamage = true;
        rb.isKinematic = false;
        textDamage.DOFade(1, 0);

        if (content != null)
        {
            content.transform.DOScaleY(1.15f, 0.05f).OnComplete(delegate
            {
                content.transform.DOScaleY(0.85f, 0.1f).OnComplete(delegate
                {
                    content.transform.DOScaleY(1f, 0.05f);
                });
            });
            
            /*content.transform.DOScaleX(0.85f, 0.125f).OnComplete(delegate
            {
                content.transform.DOScaleX(1.15f, 0.25f).OnComplete(delegate
                {
                    content.transform.DOScaleX(1f, 0.125f);
                });
            });*/
        }

        DOVirtual.Float(0f, 0.05f, 0.05f, (x) =>
        {
            HitEffect(fullBodies, x);
        }).OnComplete(delegate
        {
            DOVirtual.Float(0.05f, 0f, 0.05f, (x) =>
            {
                HitEffect(fullBodies, x);
            });
        });

        rb.AddForce(new Vector2(Random.Range(-0.55f, 0.55f), 4.25f), ForceMode2D.Impulse);
        textDamage.DOFade(0, 0.25f).SetDelay(0.25f).OnComplete(delegate
        {
            canvas.SetParent(parent);
            isShowDamage = false;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            canvas.anchoredPosition = Vector2.zero;
            rectDamage.anchoredPosition = Vector2.zero;
        });
    }

    private void OnDestroy()
    {
        textDamage.DOKill();
    }
}
