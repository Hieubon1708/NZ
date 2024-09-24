using UnityEngine;

public class EnemySkiner : MonoBehaviour
{
    public Type type;
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
        if (type == Type.SINGLE) SkinChangeBySingle();
        if (type == Type.SET) SkinChangeBySet();
    }

    public enum Type
    {
        SET, SINGLE
    }

    public void SkinChangeBySingle()
    {
        face.sprite = faces[Random.Range(0, faces.Length)];
        bodyUp.sprite = bodyUps[Random.Range(0, bodyUps.Length)];
        bodyDown.sprite = bodyDowns[Random.Range(0, bodyDowns.Length)];
        bodyDownJoint.sprite = bodyDownJoints[Random.Range(0, bodyDownJoints.Length)];
        int indexLeg = Random.Range(0, legs.Length);
        rightLeg.sprite = legs[indexLeg];
        leftLeg.sprite = legs[indexLeg];
    }
    
    public void SkinChangeBySet()
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
