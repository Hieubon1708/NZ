using UnityEngine;

public class Bone : MonoBehaviour
{
    public Transform view;
    public Transform[] bones;
    public SpriteRenderer[] spriteInfoOrigin;
    public float[] alphaOrigin;
    public BoneInfo[] tranOrigin;

    public void Awake()
    {
        bones = view.GetComponentsInChildren<Transform>(true);
        spriteInfoOrigin = view.GetComponentsInChildren<SpriteRenderer>(true);
        tranOrigin = new BoneInfo[bones.Length];
        alphaOrigin = new float[spriteInfoOrigin.Length];
        for (int i = 0; i < spriteInfoOrigin.Length; i++)
        {
            alphaOrigin[i] = spriteInfoOrigin[i].color.a;
        }
        for (int i = 0; i < bones.Length; i++)
        {
            tranOrigin[i] = new BoneInfo(bones[i].localPosition, bones[i].localRotation, bones[i].localScale, bones[i].gameObject.activeSelf);
        }
    }

    public void ResetBone()
    {
        for (int i = 0; i < spriteInfoOrigin.Length; i++)
        {
            Color color = spriteInfoOrigin[i].color;
            color.a = alphaOrigin[i];
            spriteInfoOrigin[i].color = color;
        }
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].gameObject.SetActive(tranOrigin[i].isActive);
            bones[i].localPosition = tranOrigin[i].pos;
            bones[i].localRotation = tranOrigin[i].rot;
            bones[i].localScale = tranOrigin[i].scale;
        }
    }
}
[System.Serializable]
public class BoneInfo
{
    public Vector2 pos;
    public Quaternion rot;
    public Vector3 scale;
    public bool isActive;

    public BoneInfo(Vector2 pos, Quaternion rot, Vector3 scale, bool isActive)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
        this.isActive = isActive;
    }
}
