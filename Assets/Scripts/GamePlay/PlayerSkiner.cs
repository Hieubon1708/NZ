using System;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerSkiner : MonoBehaviour
{
    Sprite[] caps = new Sprite[8];
    Sprite[] clothess = new Sprite[8];
    Sprite[] guns1 = new Sprite[8];
    Sprite[] guns2 = new Sprite[8];
    Sprite[] guns3 = new Sprite[8];
    Sprite[] booms = new Sprite[8];

    public SpriteRenderer cap;
    public SpriteRenderer clothes;
    public SpriteRenderer gun1;
    public SpriteRenderer gun2;
    public SpriteRenderer gun3;

    public SpriteAtlas spriteAtlas;

    private void Awake()
    {
        LoadSprites();
    }

    void LoadSprites()
    {
        for (int i = 0; i < 8; i++)
        {
            caps[i] = spriteAtlas.GetSprite("Hat " + (i + 1));
            clothess[i] = spriteAtlas.GetSprite("Body " + (i + 1));
            guns1[i] = spriteAtlas.GetSprite("Gun Butt " + (i + 1));
            guns2[i] = spriteAtlas.GetSprite("Gun Barrel " + (i + 1));
            guns3[i] = spriteAtlas.GetSprite("Gun Handguard " + (i + 1));
            booms[i] = spriteAtlas.GetSprite("grenade " + (i + 1));
        }
    }

    public void BoomChange()
    {
        for (int i = 0; i < PlayerController.instance.boomSpriteRenderers.Length; i++)
        {
            PlayerController.instance.boomSpriteRenderers[i].sprite = booms[EquipmentController.instance.playerInventory.boomLevel];
        }
    }

    public Vector2 GetCapPivot(int capLevel)
    {
        Vector2 pivot = caps[capLevel].pivot;
        Vector2 size = caps[capLevel].rect.size;
        return new Vector2(pivot.x / size.x, pivot.y / size.y);
    }

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

        BoomChange();
    }

    public void CapChange()
    {
        int index = EquipmentController.instance.playerInventory.capLevel;
        cap.sprite = caps[index];
    }

    public void ClothesChange()
    {
        int index = EquipmentController.instance.playerInventory.clothesLevel;
        clothes.sprite = clothess[index];
    }

    public void GunChange()
    {
        int index = EquipmentController.instance.playerInventory.gunLevel;
        gun1.sprite = guns1[index];
        gun2.sprite = guns2[index];
        gun3.sprite = guns3[index];
    }
}
