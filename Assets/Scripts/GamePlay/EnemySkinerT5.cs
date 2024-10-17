using UnityEngine;

public class EnemySkinerT5 : MonoBehaviour
{
    public SpriteRenderer head;
    public SpriteRenderer ass;
    public SpriteRenderer rightFang;
    public SpriteRenderer leftFang;
    public SpriteRenderer[] rightLeg;
    public SpriteRenderer[] leftLeg;

    public Sprite[] heads;
    public Sprite[] asses;
    public Sprite[] rightFangs;
    public Sprite[] leftFangs;
    public Sprite[] rightLegs;
    public Sprite[] leftLegs;

    public void OnEnable()
    {
        SkinChangeBySet();
    }

    public void SkinChangeBySet()
    {
        int index = Random.Range(0, heads.Length);
        head.sprite = heads[index];
        ass.sprite = asses[index];
        rightFang.sprite = rightFangs[index];
        leftFang.sprite = leftFangs[index];
        rightLeg[0].sprite = rightLegs[index];
        rightLeg[1].sprite = rightLegs[index];
        leftLeg[0].sprite = leftLegs[index];
        leftLeg[1].sprite = leftLegs[index];
    }
}
