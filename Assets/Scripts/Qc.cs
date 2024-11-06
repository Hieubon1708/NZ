using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QC : MonoBehaviour
{
    public static QC instance;
    private bool isAllowKeyCode;
    public GameObject[] UI;

    public void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.F))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1920, 1080, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.G))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1920, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.V))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1080, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.N))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1350, 1080, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.B))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1350, true);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.F))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1920, 1080, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.G))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1920, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.V))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1080, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.N))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1350, 1080, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.B))
        {
            isAllowKeyCode = true;
            Screen.SetResolution(1080, 1350, false);
            DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameController.instance.level = Mathf.Clamp(--GameController.instance.level, 0 , 1);
            DataManager.instance.SaveData();
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameController.instance.level = Mathf.Clamp(++GameController.instance.level, 0, 1);
            DataManager.instance.SaveData();
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            NextBackQuip(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextBackQuip(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameController.instance.level == 0)
            {
                for (int i = 0; i < Booster.instance.weaponBoosters.Length; i++)
                {
                    if (Booster.instance.weaponBoosters[i] is SawBooster)
                    {
                        if (!(Booster.instance.weaponBoosters[i] as SawBooster).frame.raycastTarget) return;
                        Booster.instance.weaponBoosters[i].OnClick();
                    }
                }
            }
            else
            {
                for (int i = 0; i < Booster.instance.weaponBoosters.Length; i++)
                {
                    if (Booster.instance.weaponBoosters[i] is ShockerBooster)
                    {
                        if (!(Booster.instance.weaponBoosters[i] as ShockerBooster).frame.raycastTarget) return;
                        Booster.instance.weaponBoosters[i].OnClick();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < Booster.instance.weaponBoosters.Length; i++)
            {
                if (Booster.instance.weaponBoosters[i] is FlameBooster)
                {
                    if (!(Booster.instance.weaponBoosters[i] as FlameBooster).frame.raycastTarget) return;
                    Booster.instance.weaponBoosters[i].OnClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < Booster.instance.weaponBoosters.Length; i++)
            {
                if (Booster.instance.weaponBoosters[i] as MachineGunBooster)
                {
                    if (!(Booster.instance.weaponBoosters[i] as MachineGunBooster).frame.raycastTarget) return;
                    Booster.instance.weaponBoosters[i].OnClick();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < Booster.instance.weaponBoosters.Length; i++)
            {
                if (Booster.instance.weaponBoosters[i] is BoomBooster)
                {
                    if (!(Booster.instance.weaponBoosters[i] as BoomBooster).frame.raycastTarget) return;
                    Booster.instance.weaponBoosters[i].OnClick();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Booster.instance.PlusEnergy();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlusGem();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlusGold();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < UI.Length; i++)
            {
                UI[i].SetActive(!UI[i].activeSelf);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerController.instance.traectory.SetActive(!PlayerController.instance.traectory.activeSelf);
        }

        /*        if (Input.GetKeyDown(KeyCode.F4))
                {
                    if (!BlockController.instance.blockBuyer.frameButton.raycastTarget) return;
                    BlockController.instance.blockBuyer.Buy();
                }
                if (Input.GetKeyDown(KeyCode.F5))
                {
                    if (!BlockController.instance.energyUpgradee.frame.raycastTarget) return;
                    BlockController.instance.energyUpgradee.Upgrade();
                }
                if (Input.GetKeyDown(KeyCode.F6))
                {
                    if (!UIHandler.instance.frameRewardGold.raycastTarget) return;
                    UIHandler.instance.RewardGold();
                }

                if (Input.GetKeyDown(KeyCode.F7))
                {
                    UpgradeBlock(0);
                }
                if (Input.GetKeyDown(KeyCode.F8))
                {
                    UpgradeBlock(1);
                }
                if (Input.GetKeyDown(KeyCode.F9))
                {
                    UpgradeBlock(2);
                }
                if (Input.GetKeyDown(KeyCode.F10))
                {
                    UpgradeBlock(3);
                }
                if (Input.GetKeyDown(KeyCode.F11))
                {
                    UpgradeBlock(4);
                }
                if (Input.GetKeyDown(KeyCode.F12))
                {
                    UpgradeBlock(5);
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    UpgradeWeapon(0);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    UpgradeWeapon(1);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    UpgradeWeapon(2);
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    UpgradeWeapon(3);
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    UpgradeWeapon(4);
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    UpgradeWeapon(5);
                }
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    DestroyBlock(0);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    DestroyBlock(1);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    DestroyBlock(2);
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    DestroyBlock(3);
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    DestroyBlock(4);
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    DestroyBlock(5);
                }

                if (Input.GetKeyDown(KeyCode.J))
                {

                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    if (Input.GetKeyDown(KeyCode.J))
                    {

                    }
                }
                if (Input.GetKeyDown(KeyCode.L))
                {

                }*/
    }

    void DestroyBlock(int index)
    {
        Block scBlock = BlockController.instance.GetScBlock(BlockController.instance.blocks[index]);
        if (scBlock == null) return;
        scBlock.SubtractGold();
        scBlock.blockUpgradeHandler.ResetData();
        BlockController.instance.CheckButtonStateAll();
    }

    void UpgradeBlock(int index)
    {
        Block scBlock = BlockController.instance.GetScBlock(BlockController.instance.blocks[index]);
        if (scBlock == null || scBlock.blockUpgradeHandler.weaponUpgradeHandler.textMax.gameObject.activeSelf || !scBlock.blockUpgradeHandler.frame.raycastTarget) return;
        scBlock.blockUpgradeHandler.Upgrade();
    }

    void UpgradeWeapon(int index)
    {
        Block scBlock = BlockController.instance.GetScBlock(BlockController.instance.blocks[index]);
        Debug.LogWarning(scBlock.name);
        if (scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter == null || scBlock.blockUpgradeHandler.weaponUpgradeHandler.textMax.gameObject.activeSelf || !scBlock.blockUpgradeHandler.weaponUpgradeHandler.frame.raycastTarget) return;
        scBlock.blockUpgradeHandler.weaponUpgradeHandler.Upgrade();
    }

    void PlusGold()
    {
        PlayerController.instance.player.gold += 5000;
        UIHandler.instance.GoldUpdatee();
        BlockController.instance.CheckButtonStateAll();
        DataManager.instance.SaveData();
    }

    void PlusGem()
    {
        EquipmentController.instance.playerInventory.gem += 90;
        UIHandler.instance.summonEquipment.RewardGem();
    }


    void NextBackQuip(bool isNext)
    {
        if (isNext)
        {
            EquipmentController.instance.playerInventory.gunLevel = Mathf.Clamp(++EquipmentController.instance.playerInventory.gunLevel, 0, 7);
            EquipmentController.instance.playerInventory.boomLevel = Mathf.Clamp(++EquipmentController.instance.playerInventory.boomLevel, 0, 7);
            EquipmentController.instance.playerInventory.capLevel = Mathf.Clamp(++EquipmentController.instance.playerInventory.capLevel, 0, 7);
            EquipmentController.instance.playerInventory.clothesLevel = Mathf.Clamp(++EquipmentController.instance.playerInventory.clothesLevel, 0, 7);
        }
        else
        {
            EquipmentController.instance.playerInventory.gunLevel = Mathf.Clamp(--EquipmentController.instance.playerInventory.gunLevel, 0, 7);
            EquipmentController.instance.playerInventory.boomLevel = Mathf.Clamp(--EquipmentController.instance.playerInventory.boomLevel, 0, 7);
            EquipmentController.instance.playerInventory.capLevel = Mathf.Clamp(--EquipmentController.instance.playerInventory.capLevel, 0, 7);
            EquipmentController.instance.playerInventory.clothesLevel = Mathf.Clamp(--EquipmentController.instance.playerInventory.clothesLevel, 0, 7);
        }
        for (int i = 0; i < EquipmentController.instance.equipMains.Length; i++)
        {
            int level = EquipmentController.instance.GetIndexLevel(i);
            EquipmentController.instance.SetEquip(i, level, EquipmentController.instance.equipMains[i]);
            EquipmentController.instance.equipCurrentLevels[i].text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(EquipmentController.instance.GetLevelUpgrade(EquipmentController.instance.equipMains[i].type) + 1);
        }
        PlayerController.instance.player.playerSkiner.LoadData();
        BulletController.instance.SetDamage(EquipmentController.instance.GetEquipValue(EquipmentController.EQUIPMENTTYPE.SHOTGUN, EquipmentController.instance.playerInventory.gunLevel, EquipmentController.instance.playerInventory.gunLevelUpgrade));
        PlayerController.instance.player.HpChange();
        PlayerController.instance.playerHandler.LoadData();
    }
}
