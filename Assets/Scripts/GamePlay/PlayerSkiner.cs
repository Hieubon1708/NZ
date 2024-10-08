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
        cap.sprite = caps[EquipmentController.instance.indexCap];
    }

    public void ClothesChange()
    {
        clothes.sprite = clothess[EquipmentController.instance.indexClothes];
    }

    public void GunChange()
    {
        gun.sprite = guns[EquipmentController.instance.indexGun];
    }
}
