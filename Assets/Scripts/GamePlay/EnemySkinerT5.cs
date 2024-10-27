using UnityEngine;
using UnityEngine.U2D;

public class EnemySkinerT5 : MonoBehaviour
{
    public int level;
    public int amountSet;

    public SpriteRenderer head;
    public SpriteRenderer ass;
    public SpriteRenderer rightFang;
    public SpriteRenderer leftFang;
    public SpriteRenderer[] rightLeg;
    public SpriteRenderer[] leftLeg;

    Sprite[] heads;
    Sprite[] asses;
    Sprite[] rightFangs;
    Sprite[] leftFangs;
    Sprite[] rightLegs;
    Sprite[] leftLegs;

    public SpriteAtlas spriteAtlas;

    private void Awake()
    {
        LoadSprites();
    }

    public void OnEnable()
    {
        SkinChangeBySet();
    }

    void LoadSprites()
    {
        heads = new Sprite[amountSet];
        asses = new Sprite[amountSet];
        rightFangs = new Sprite[amountSet];
        leftFangs = new Sprite[amountSet];
        rightLegs = new Sprite[amountSet];
        leftLegs = new Sprite[amountSet];
        for (int i = 0; i < amountSet; i++)
        {
            heads[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Head");
            asses[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Ass");
            rightFangs[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Fang_Right");
            leftFangs[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Fang_Left");
            rightLegs[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Leg_Right");
            leftLegs[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Leg_Left");
        }
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
