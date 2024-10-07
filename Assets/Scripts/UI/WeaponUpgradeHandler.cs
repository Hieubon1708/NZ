using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeHandler : ButtonUpgradee
{
    public int level;
    public int levelUpgrade;
    public TextMeshProUGUI textLv;
    public TextMeshProUGUI textDamage;
    public Image frameLastUpgrade;
    public TextMeshProUGUI textMax;
    public TextMeshProUGUI textLastUpgrade;
    public GameObject lastUpgrade;
    public GameObject[] boxProgress;
    public BlockUpgradeHandler blockUpgradeHandler;
    public WeaponShoter weaponShoter;
    public WeaponConfig weaponConfig;

    public void LoadData(int level, int levelUpgrade)
    {
        this.levelUpgrade = levelUpgrade;
        this.level = level;
    }

    public void StartGame()
    {
        if (weaponShoter != null)
        {
            weaponShoter.ani.SetBool("startGame", true);
            weaponShoter.StartGame();
        }
    }

    public void SetWeaponShoterNConfig(WeaponShoter weaponShoter)
    {
        this.weaponShoter = weaponShoter;
        weaponConfig = DataManager.instance.FindWeaponConfigByType(weaponShoter.weaponType);
        if (weaponConfig == null) Debug.LogError("!");
        UpgradeHandle();
    }

    public override void Upgrade()
    {
        blockUpgradeHandler.blockInfo.PlusGold(DataManager.instance.GetUpgradePriceWeaponConfig(level, levelUpgrade, weaponConfig));
        levelUpgrade++;
        if (lastUpgrade.activeSelf)
        {
            level++;
            levelUpgrade = 0;
            blockUpgradeHandler.BuyWeapon(weaponShoter.weaponType, level);
            lastUpgrade.SetActive(false);
            if (weaponShoter.weaponType == GameController.WEAPON.SAW) UIUpgradeEvolution.instance.ShowPanelSawEvo();
            if (weaponShoter.weaponType == GameController.WEAPON.FLAME) UIUpgradeEvolution.instance.ShowPanelFlameEvo();
            if (weaponShoter.weaponType == GameController.WEAPON.MACHINE_GUN) UIUpgradeEvolution.instance.ShowPanelMachineGunEvo();
        }
        UpgradeHandle();
    }

    public override void UpgradeHandle()
    {
        int damage = DataManager.instance.GetDamageWeaponConfig(level, levelUpgrade, weaponConfig);
        int priceUpgrade = DataManager.instance.GetUpgradePriceWeaponConfig(level, levelUpgrade, weaponConfig);
        int lengthPriceUpgrade = DataManager.instance.GetLengthUpgradePriceWeaponConfig(level, weaponConfig);
        int damageBooster = DataManager.instance.GetDamageBoosterWeaponConfig(level, weaponConfig);

        weaponShoter.SetDamageBooster(damageBooster);
        weaponShoter.SetDamage(damage);

        if (levelUpgrade == boxProgress.Length && level != lengthPriceUpgrade - 1) ChangeTextUpgrade();
        else
        {
            textLv.text = "Lv" + (level + 1);
            textDamage.text = damage.ToString();
            textPriceUpgrade.text = priceUpgrade.ToString();
        }

        for (int i = 0; i < boxProgress.Length; i++)
        {
            if (i > levelUpgrade - 1 || levelUpgrade == boxProgress.Length && level == lengthPriceUpgrade - 1) boxProgress[i].SetActive(false);
            else boxProgress[i].SetActive(true);
        }
    }

    void ChangeTextUpgrade()
    {
        lastUpgrade.SetActive(true);
        textLastUpgrade.text = "Lv" + (level + 1) + " > " + "Lv" + (level + 2) + " UPGRADE";
        textPriceUpgrade.text = DataManager.instance.GetEvolutionPriceWeaponConfig(level, weaponConfig).ToString();
    }

    public override void CheckButtonState()
    {
        if (weaponShoter == null) return;
        if (level == DataManager.instance.GetLengthUpgradePriceWeaponConfig(level, weaponConfig) - 1 && levelUpgrade == boxProgress.Length - 1)
        {
            UIHandler.instance.ChangeSpriteWeaponUpgradee(frame, textPriceUpgrade, textMax);
        }
        else if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.GetUpgradePriceWeaponConfig(level, levelUpgrade, weaponConfig))
        {
            if (levelUpgrade == boxProgress.Length) UIHandler.instance.ChangeSpriteWeaponLastUpgradee(UIHandler.Type.NOT_ENOUGH_MONEY, frameLastUpgrade);
            else UIHandler.instance.ChangeSpriteWeaponUpgradee(UIHandler.Type.NOT_ENOUGH_MONEY, frame);
        }
        else
        {
            if (levelUpgrade == boxProgress.Length) UIHandler.instance.ChangeSpriteWeaponLastUpgradee(UIHandler.Type.ENOUGH_MONEY, frameLastUpgrade);
            else UIHandler.instance.ChangeSpriteWeaponUpgradee(UIHandler.Type.ENOUGH_MONEY, frame);
        }
    }

    public void ResetData()
    {
        if (weaponShoter == null) return;
        weaponShoter.parent.gameObject.SetActive(false);
        weaponShoter = null;
        weaponConfig = null;
        level = 0;
        levelUpgrade = 0;
        lastUpgrade.SetActive(false);
    }
}
