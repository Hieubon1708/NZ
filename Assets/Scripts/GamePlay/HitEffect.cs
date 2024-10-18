using DG.Tweening;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    bool isHit;

    public void PlayHitEffect(SpriteRenderer[] fullBodies)
    {
        if (isHit) return;
        isHit = true;
        DOVirtual.Float(0f, 0.03f, 0.1f, (x) =>
        {
            ChangeMaterial(fullBodies, x);
        }).OnComplete(delegate
        {
            DOVirtual.Float(0.03f, 0f, 0.1f, (x) =>
            {
                ChangeMaterial(fullBodies, x);
            });
        });
        DOVirtual.DelayedCall(0.3f, delegate { isHit = false; });
    }

    void ChangeMaterial(SpriteRenderer[] fullBodies, float value)
    {
        for (int i = 0; i < fullBodies.Length; i++)
        {
            fullBodies[i].material.SetFloat("_HitEffectBlend", value);
        }
    }
}
