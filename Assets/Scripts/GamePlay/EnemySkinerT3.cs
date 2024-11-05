using UnityEngine;
using UnityEngine.U2D;

public class EnemySkinerT3 : MonoBehaviour
{
    public int level;
    public int amountSet;

    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer ass;
    public SpriteRenderer sting;
    public SpriteRenderer rightLeg;
    public SpriteRenderer leftLeg;

    Sprite[] heads;
    Sprite[] bodys;
    Sprite[] asses;
    Sprite[] stings;
    Sprite[] rightLegs;
    Sprite[] leftLegs;

    public SpriteAtlas spriteAtlas;

    private void Awake()
    {
        LoadSprites();
    }

    public void OnEnable()
    {
        SkinChange();
    }

    void LoadSprites()
    {
        heads = new Sprite[amountSet];
        bodys = new Sprite[amountSet];
        asses = new Sprite[amountSet];
        stings = new Sprite[amountSet];
        rightLegs = new Sprite[amountSet];
        leftLegs = new Sprite[amountSet];
        for (int i = 0; i < amountSet; i++)
        {
            heads[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Head");
            bodys[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Body");
            asses[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Ass");
            stings[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Sting");
            rightLegs[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Wings_Left");
            leftLegs[i] = spriteAtlas.GetSprite("Spider_" + level + "_" + (i + 1) + "_Wings_Right");
        }
    }

    public void SkinChange()
    {
        int index = Random.Range(0, heads.Length);
        head.sprite = heads[index];
        body.sprite = bodys[index];
        ass.sprite = asses[index];
        sting.sprite = stings[index];
        rightLeg.sprite = leftLegs[index];
        leftLeg.sprite = rightLegs[index];
    }
}
