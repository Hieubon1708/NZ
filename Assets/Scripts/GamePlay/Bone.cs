using UnityEngine;

public class Bone : MonoBehaviour
{
    public Transform view;
    public Transform[] bones;
    public SpriteRenderer[] spriteInfoOrigin;
    public BoneTran[] infoOrigin;

    public void Start()
    {
        bones = view.GetComponentsInChildren<Transform>();
        spriteInfoOrigin = view.GetComponentsInChildren<SpriteRenderer>();
        infoOrigin = new BoneTran[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            infoOrigin[i] = new BoneTran(bones[i].localPosition, bones[i].localRotation);
        }
    }

    public void ResetBone()
    {
        for (int i = 0; i < spriteInfoOrigin.Length; i++)
        {
            Color color = spriteInfoOrigin[i].color;
            color.a = 1;
            spriteInfoOrigin[i].color = color;
        }
        for (int i = 0; i < bones.Length; i++)
        {
            if (!bones[i].gameObject.activeSelf) bones[i].gameObject.SetActive(true);
            bones[i].localPosition = infoOrigin[i].pos;
            bones[i].localRotation = infoOrigin[i].rot;
            bones[i].localScale = Vector3.one;
        }
    }
}

public class BoneTran
{
    public Vector2 pos;
    public Quaternion rot;

    public BoneTran(Vector2 pos, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
    }
}
