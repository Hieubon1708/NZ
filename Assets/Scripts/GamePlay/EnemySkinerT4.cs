using UnityEngine;
using UnityEngine.U2D;

public class EnemySkinerT4 : MonoBehaviour
{
    public int level;
    public int amountSet;

    public SpriteRenderer head;
    public SpriteRenderer ass;
    public SpriteRenderer[] rightLegUp;
    public SpriteRenderer[] rightLegDown;
    public SpriteRenderer[] leftLegUp;
    public SpriteRenderer[] leftLegDown;

    Sprite[] heads;
    Sprite[] asses;
    Sprite[] rightLegUps;
    Sprite[] rightLegDowns;
    Sprite[] leftLegUps;
    Sprite[] leftLegDowns;

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
        rightLegUps = new Sprite[amountSet];
        rightLegDowns = new Sprite[amountSet];
        leftLegUps = new Sprite[amountSet];
        leftLegDowns = new Sprite[amountSet];
        for (int i = 0; i < amountSet; i++)
        {
            heads[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Head");
            asses[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Ass");
            rightLegUps[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Leg_Right_Up");
            rightLegDowns[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Leg_Right_Down");
            leftLegUps[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Leg_Left_Up");
            leftLegDowns[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Leg_Left_Down");
        }
    }

    public void SkinChangeBySet()
    {
        int index = Random.Range(0, heads.Length);
        head.sprite = heads[index];
        ass.sprite = asses[index];

        SpriteChange(rightLegUp, rightLegUps[index]);
        SpriteChange(rightLegDown, rightLegDowns[index]);
        SpriteChange(leftLegUp, leftLegUps[index]);
        SpriteChange(leftLegDown, leftLegDowns[index]);
    }

    void SpriteChange(SpriteRenderer[] sps, Sprite sp)
    {
        for ( int i = 0; i < sps.Length; i++ )
        {
            sps[i].sprite = sp;
        }
    }
}
