using System;
using UnityEngine;

public class PlayerSkiner : MonoBehaviour
{
    public Sprite[] caps;
    public Sprite[] clothess;
    public Sprite[] guns1;
    public Sprite[] guns2;
    public Sprite[] guns3;
    public Sprite[] booms;

    public SpriteRenderer cap;
    public SpriteRenderer clothes;
    public SpriteRenderer gun1;
    public SpriteRenderer gun2;
    public SpriteRenderer gun3;

    public void LoadData()
    {
        int indexCap = EquipmentController.instance.playerInventory.capLevel;
        int indexClothes = EquipmentController.instance.playerInventory.clothesLevel;
        int indexGun = EquipmentController.instance.playerInventory.gunLevel;

        cap.sprite = caps[indexCap];
        clothes.sprite = clothess[indexClothes];

        gun1.sprite = guns1[indexGun];
        gun2.sprite = guns2[indexGun];
        gun3.sprite = guns3[indexGun];

        PlayerController.instance.BoomSkinChange();
    }

    public void CapChange(int index)
    {
        cap.sprite = caps[index];
    }

    public void ClothesChange(int index)
    {
        clothes.sprite = clothess[index];
    }

    public void GunChange(int index)
    {
        gun1.sprite = guns1[index];
        gun2.sprite = guns2[index];
        gun3.sprite = guns3[index];
    }
}
