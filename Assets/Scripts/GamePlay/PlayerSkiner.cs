using UnityEngine;

public class PlayerSkiner : MonoBehaviour
{
    public Sprite[] caps;
    public Sprite[] clothess;
    public Sprite[] guns;
    public Sprite[] booms;

    public SpriteRenderer cap;
    public SpriteRenderer clothes;
    public SpriteRenderer gun;

    public void CapChange()
    {
        cap.sprite = caps[PlayerInventory.instance.capLevel];
    }

    public void ClothesChange()
    {
        clothes.sprite = clothess[PlayerInventory.instance.clothesLevel];
    }

    public void GunChange()
    {
        gun.sprite = guns[PlayerInventory.instance.gunLevel];
    }
}
