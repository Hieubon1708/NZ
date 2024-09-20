using UnityEngine;

public class EnemySkiner : MonoBehaviour
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
        face.sprite = faces[Random.Range(0, faces.Length)];
        bodyUp.sprite = bodyUps[Random.Range(0, bodyUps.Length)];
        bodyDown.sprite = bodyDowns[Random.Range(0, bodyDowns.Length)];
        bodyDownJoint.sprite = bodyDownJoints[Random.Range(0, bodyDownJoints.Length)];
        int indexLeg = Random.Range(0, legs.Length);
        rightLeg.sprite = legs[indexLeg];
        leftLeg.sprite = legs[indexLeg];
    }
}
