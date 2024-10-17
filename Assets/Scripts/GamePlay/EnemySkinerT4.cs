using UnityEngine;

public class EnemySkinerT4 : MonoBehaviour
{
    public SpriteRenderer head;
    public SpriteRenderer ass;
    public SpriteRenderer[] rightLegUp;
    public SpriteRenderer[] rightLegDown;
    public SpriteRenderer[] leftLegUp;
    public SpriteRenderer[] leftLegDown;

    public Sprite[] heads;
    public Sprite[] asses;
    public Sprite[] rightLegUps;
    public Sprite[] rightLegDowns;
    public Sprite[] leftLegUps;
    public Sprite[] leftLegDowns;

    public void OnEnable()
    {
        SkinChangeBySet();
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
