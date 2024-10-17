using UnityEngine;

public class EnemySkinerT3 : MonoBehaviour
{
    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer ass;
    public SpriteRenderer sting;
    public SpriteRenderer rightLeg;
    public SpriteRenderer leftLeg;

    public Sprite[] heads;
    public Sprite[] bodys;
    public Sprite[] asses;
    public Sprite[] stings;
    public Sprite[] rightLegs;
    public Sprite[] leftLegs;

    public void OnEnable()
    {
        SkinChange();
    }
    
    public void SkinChange()
    {
        int index = Random.Range(0, heads.Length);
        head.sprite = heads[index];
        body.sprite = bodys[index];
        ass.sprite = asses[index];
        sting.sprite = stings[index];
        rightLeg.sprite = rightLegs[index];
        leftLeg.sprite = leftLegs[index];
    }
}
