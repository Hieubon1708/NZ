using UnityEngine;

public class EnemySkinerT1 : MonoBehaviour
{
    public SpriteRenderer face;
    public SpriteRenderer bodyUp;
    public SpriteRenderer bodyDown;
    public SpriteRenderer bodyDownJoint;
    public SpriteRenderer rightLeg;
    public SpriteRenderer leftLeg;

    public Sprite[] faces;
    public Sprite[] bodyUps;
    public Sprite[] bodyDowns;
    public Sprite[] bodyDownJoints;
    public Sprite[] legs;

    public void OnEnable()
    {
        SkinChange();
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
