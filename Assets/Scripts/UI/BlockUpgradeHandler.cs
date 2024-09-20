using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static GameController;

public class BlockUpgradeHandler : ButtonUpgradee
{
    public Block blockInfo;
    public GameObject weaponBuyer;
    public WeaponBuyButton[] weaponBuyButtons;
    public SpriteRenderer spriteRenderer;
    public SortingGroup sortingGroup;
    public TextMeshProUGUI textLv;
    public TextMeshProUGUI textHp;
    public TextMeshProUGUI textMax;
    public Image frameButton;
    public GameObject[] saws;
    public GameObject[] flames;
    public GameObject[] machineGuns;
    public GameObject weaponUpgrade;
    public GameObject canvas;
    public WeaponUpgradeHandler weaponUpgradeHandler;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckButtonStateInBlock();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Upgrade();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (DataManager.instance.playerData.gold < DataManager.instance.sawData.price) return;
            BuyWeapon(WEAPON.SAW, 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (DataManager.instance.playerData.gold < DataManager.instance.flameData.price) return;
            BuyWeapon(WEAPON.FLAME, 0);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (DataManager.instance.playerData.gold < DataManager.instance.machineGunData.price) return;
            BuyWeapon(WEAPON.MACHINE_GUN, 0);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetData();
        }
    }

    public void LoadData(int blockLevel, WEAPON weaponType, int weaponLevel, int levelUpgrade)
    {
        blockInfo.level = blockLevel;
        UpgradeHandle();
        LoadData();
        if (weaponType == WEAPON.NONE) return;
        weaponUpgradeHandler.LoadData(weaponLevel, levelUpgrade);
        BuyWeapon(weaponType, weaponLevel);
    }

    public void LoadData()
    {
        for (int i = 0; i < weaponBuyButtons.Length; i++)
        {
            weaponBuyButtons[i].LoadData();
        }
    }

    public void BuyWeapon(WEAPON weaponType, int weaponLevel)
    {
        GameObject[] weapons = null;
        weaponUpgradeHandler.weaponType = weaponType;
        if (weaponType == WEAPON.SAW)
        {
            weapons = saws;
            weaponUpgradeHandler.priceUpgrades = DataManager.instance.sawData.priceUpgrades;
            weaponUpgradeHandler.damages = DataManager.instance.sawData.damages;
        }
        if (weaponType == WEAPON.FLAME)
        {
            weapons = flames;
            weaponUpgradeHandler.priceUpgrades = DataManager.instance.flameData.priceUpgrades;
            weaponUpgradeHandler.damages = DataManager.instance.flameData.damages;
        }
        if (weaponType == WEAPON.MACHINE_GUN)
        {
            weapons = machineGuns;
            weaponUpgradeHandler.priceUpgrades = DataManager.instance.machineGunData.priceUpgrades;
            weaponUpgradeHandler.damages = DataManager.instance.machineGunData.damages;
        }
        weaponBuyer.SetActive(false);
        weaponUpgrade.SetActive(true);
        if(weaponLevel > 0) weapons[weaponLevel -1].SetActive(false);
        weapons[weaponLevel].SetActive(true);
        weaponUpgradeHandler.weapon = weapons[weaponLevel];
        weaponUpgradeHandler.UpgradeHandle();
    }

    public override void Upgrade()
    {
        blockInfo.PlusGold(DataManager.instance.blockData.priceUpgrades[blockInfo.level]);
        blockInfo.level++;
        UpgradeHandle();
    }

    public void CheckButtonStateInBlock()
    {
        for(int i = 0; i < weaponBuyButtons.Length; i++)
        {
            weaponBuyButtons[i].CheckButtonState();
        }
        CheckButtonState();
        weaponUpgradeHandler.CheckButtonState();
    }

    public override void CheckButtonState()
    {
        if (blockInfo.level == DataManager.instance.blockData.hps.Length - 1) UIHandler.instance.ChangeSpriteBlockUpgradee(frameButton, textPriceUpgrade, textMax);
        else if (DataManager.instance.playerData.gold < DataManager.instance.blockData.priceUpgrades[blockInfo.level]) UIHandler.instance.ChangeSpriteBlockUpgradee(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton);
        else UIHandler.instance.ChangeSpriteBlockUpgradee(UIHandler.Type.ENOUGH_MONEY, frameButton);
    }

    public override void UpgradeHandle()
    {
        textLv.text = "Lv" + (blockInfo.level + 1);
        spriteRenderer.sprite = DataManager.instance.blockSprites[blockInfo.level];
        int hp = DataManager.instance.blockData.hps[blockInfo.level];
        textHp.text = hp >= 1000 ? Mathf.Floor(hp / 100) / 10 + "K" : hp.ToString();
        textPriceUpgrade.text = DataManager.instance.blockData.priceUpgrades[blockInfo.level].ToString();
    }

    public void ResetData()
    {
        blockInfo.level = 0;
        weaponBuyer.SetActive(true);
        weaponUpgrade.SetActive(false);
        textPriceUpgrade.gameObject.SetActive(true);
        textMax.gameObject.SetActive(false);

        weaponUpgradeHandler.ResetData();
    }

    public void Selected()
    {
        canvas.SetActive(false);
        transform.localScale = Vector3.one * 1.55f;
        sortingGroup.sortingLayerName = "UI";
    }

    public void DeSelected()
    {
        canvas.SetActive(true);
        transform.localScale = Vector3.one;
        sortingGroup.sortingLayerName = "Default";
    }
}
