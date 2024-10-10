using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static GameController;

public class BlockUpgradeHandler : ButtonUpgradee
{
    public Block blockInfo;
    public GameObject weaponBuyer;
    public BlockHandler blockHandler;
    public WeaponBuyButton[] weaponBuyButtons;
    public SpriteRenderer spriteRenderer;
    public SortingGroup sortingGroup;
    public TextMeshProUGUI textLv;
    public TextMeshProUGUI textHpR;
    public TextMeshProUGUI textHpL;
    public TextMeshProUGUI textMax;
    public Image frameButton;
    public Image framePrice;
    public Image iconGold;
    public GameObject[] saws;
    public GameObject[] flames;
    public GameObject[] machineGuns;
    public GameObject weaponUpgrade;
    public GameObject canvas;
    public WeaponUpgradeHandler weaponUpgradeHandler;

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

        if (weaponType == WEAPON.SAW) weapons = saws;
        if (weaponType == WEAPON.FLAME) weapons = flames;
        if (weaponType == WEAPON.MACHINE_GUN) weapons = machineGuns;

        weaponBuyer.SetActive(false);
        weaponUpgrade.SetActive(true);

        if (weaponLevel > 0) weapons[weaponLevel - 1].SetActive(false);
        weapons[weaponLevel].SetActive(true);

        weaponUpgradeHandler.SetWeaponShoterNConfig(weapons[weaponLevel].GetComponentInChildren<WeaponShoter>());
    }

    public override void Upgrade()
    {
        blockInfo.PlusGold(DataManager.instance.blockConfig.priceUpgrades[blockInfo.level]);
        blockInfo.level++;
        blockInfo.UpgradeBlockAni();
        UpgradeHandle();
    }

    public void CheckButtonStateInBlock()
    {
        for (int i = 0; i < weaponBuyButtons.Length; i++)
        {
            weaponBuyButtons[i].CheckButtonState();
        }
        CheckButtonState();
        weaponUpgradeHandler.CheckButtonState();
    }

    public override void CheckButtonState()
    {
        if (blockInfo.level == DataManager.instance.blockConfig.hpUpgrades.Length - 1) UIHandler.instance.BlockButtonChangeState(frameButton, framePrice, textPriceUpgrade, textMax, iconGold, textLv, textHpL, textHpR);
        else if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.blockConfig.priceUpgrades[blockInfo.level]) UIHandler.instance.BlockButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, framePrice, textLv, textHpL, textHpR);
        else UIHandler.instance.BlockButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frameButton, framePrice, textLv, textHpL, textHpR);
    }

    public override void UpgradeHandle()
    {
        textLv.text = "Lv " + (blockInfo.level + 1);
        blockInfo.hp = DataManager.instance.blockConfig.hpUpgrades[blockInfo.level];
        blockHandler.SetTotalHp();
        spriteRenderer.sprite = DataManager.instance.blockSprites[blockInfo.level];
        int hp = DataManager.instance.blockConfig.hpUpgrades[blockInfo.level];
        textHpR.text = UIHandler.instance.ConvertNumberAbbreviation(hp);
        textPriceUpgrade.text = DataManager.instance.blockConfig.priceUpgrades[blockInfo.level].ToString();
    }

    public void ResetData()
    {
        blockInfo.level = 0;
        weaponBuyer.SetActive(true);
        iconGold.gameObject.SetActive(true);
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
