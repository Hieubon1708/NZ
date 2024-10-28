using UnityEngine;
using UnityEngine.U2D;

public class EnemySkinerT1 : MonoBehaviour
{
    public int level;
    public int amountSet;

    public SpriteRenderer face;
    public SpriteRenderer bodyUp;
    public SpriteRenderer bodyDown;
    public SpriteRenderer bodyDownJoint;
    public SpriteRenderer rightLeg;
    public SpriteRenderer leftLeg;

    Sprite[] faces;
    Sprite[] bodyUps;
    Sprite[] bodyDowns;
    Sprite[] bodyDownJoints;
    Sprite[] legs;

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
        faces = new Sprite[amountSet];
        bodyUps = new Sprite[amountSet];
        bodyDowns = new Sprite[amountSet];
        bodyDownJoints = new Sprite[amountSet];
        legs = new Sprite[amountSet];
        for (int i = 0; i < amountSet; i++)
        {
            faces[i] = spriteAtlas.GetSprite("zombie_" + level + "_" + (i + 1) + "_head");
            bodyUps[i] = spriteAtlas.GetSprite("zombie_" + level + "_" + (i + 1) + "_body_up");
            bodyDowns[i] = spriteAtlas.GetSprite("zombie_" + level + "_" + (i + 1) + "_body_down");
            bodyDownJoints[i] = spriteAtlas.GetSprite("zombie_" + level + "_" + (i + 1) + "_body_down_joint");
            legs[i] = spriteAtlas.GetSprite("zombie_" + level + "_" + (i + 1) + "_leg");
        }
    }
    
    public void SkinChange()
    {
        int index = Random.Range(0, faces.Length);
        face.sprite = faces[index];
        bodyUp.sprite = bodyUps[index];
        bodyDown.sprite = bodyDowns[index];
        bodyDownJoint.sprite = bodyDownJoints[index];
        rightLeg.sprite = legs[index];
        leftLeg.sprite = legs[index];
    }
}
