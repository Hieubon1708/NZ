using UnityEngine;
using DG.Tweening;

public class HealthHandler : MonoBehaviour
{
    public SpriteRenderer layer_1;
    public SpriteRenderer layer_2;
    float startWidth;
    public int startHp;
    public float timeSubtractHp_1;
    public float timeSubtractHp_2;
    Tween delayCallLayer_1;
    Tween delayCallLayer_2;
    bool isCompleted = true;

    public void Awake()
    {
        startWidth = layer_2.size.x;
    }

    public void SetDefaultInfo(ref int hp)
    {
        KillDelay();
        hp = startHp;
        layer_1.size = new Vector2(startWidth, layer_1.size.y);
        layer_2.size = new Vector2(startWidth, layer_2.size.y);
    }

    public void SetTotalHp(int totalHp)
    {
        startHp = totalHp;
    }

    public void SubtractHp(float currentHp)
    {
        delayCallLayer_2.Kill();
        float percentage = GetPercentageOfTotal(currentHp, startHp);
        float value = GetValueOfPercentage(percentage, startWidth);
        delayCallLayer_2 = DOVirtual.Float(layer_2.size.x, value, timeSubtractHp_2, (x) =>
        {
            layer_2.size = new Vector2(x, layer_2.size.y);
        });

        DOVirtual.DelayedCall(0.1f, delegate
        {
            delayCallLayer_1.Kill();
            delayCallLayer_1 = DOVirtual.Float(layer_1.size.x, value, timeSubtractHp_1, (x) =>
            {
                layer_1.size = new Vector2(x, layer_1.size.y);
            });
        });
    }

    float GetPercentageOfTotal(float value, float total)
    {
        return value / total * 100;
    }

    float GetValueOfPercentage(float percentage, float total)
    {
        return percentage * total / 100;
    }

    void KillDelay()
    {
        delayCallLayer_2.Kill();
        delayCallLayer_1.Kill();
    }

    private void OnDisable()
    {
        KillDelay();
    }
}
