using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeHandler : ButtonUpgradee
{
    public int level;
    public int levelUpgrade;
    public TextMeshProUGUI textLv;
    public TextMeshProUGUI textDamageR;
    public TextMeshProUGUI textDamageL;
    public Image frameEvoUpgrade;
    public Image[] boxes;
    public TextMeshProUGUI textMax;
    public TextMeshProUGUI textEvoUpgrade;
    public GameObject evoUpgrade;
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
        if (evoUpgrade.activeSelf)
        {
            level++;
            levelUpgrade = 0;
            blockUpgradeHandler.BuyWeapon(weaponShoter.weaponType, level);
            evoUpgrade.SetActive(false);
            if (weaponShoter.weaponType == GameController.WEAPON.SAW) UpgradeEvolutionController.instance.uIUpgradeEvolution.ShowPanelSawEvo();
            if (weaponShoter.weaponType == GameController.WEAPON.FLAME) UpgradeEvolutionController.instance.uIUpgradeEvolution.ShowPanelFlameEvo();
            if (weaponShoter.weaponType == GameController.WEAPON.MACHINE_GUN) UpgradeEvolutionController.instance.uIUpgradeEvolution.ShowPanelMachineGunEvo();
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
            textLv.text = "Lv " + (level + 1);
            textDamageR.text = damage.ToString();
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
        evoUpgrade.SetActive(true);
        textEvoUpgrade.text = "Lv" + (level + 1) + " > " + "Lv" + (level + 2) + " EVOLUTION";
        textPriceUpgrade.text = DataManager.instance.GetEvolutionPriceWeaponConfig(level, weaponConfig).ToString();
    }

    public override void CheckButtonState()
    {
        if (weaponShoter == null) return;
        if (level == DataManager.instance.GetLengthUpgradePriceWeaponConfig(level, weaponConfig) - 1 && levelUpgrade == boxProgress.Length - 1)
        {
            UIHandler.instance.WeaponButtonChangeState(frame, textPriceUpgrade, textMax);
        }
        else if (PlayerController.instance.player.gold < DataManager.instance.GetUpgradePriceWeaponConfig(level, levelUpgrade, weaponConfig))
        {
            if (levelUpgrade == boxProgress.Length) UIHandler.instance.WeaponEvoButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frameEvoUpgrade, framePrice, arrow);
            else UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frame, framePrice, textLv, textDamageL, textDamageR, boxes, arrow);
        }
        else
        {
            if (levelUpgrade == boxProgress.Length) UIHandler.instance.WeaponEvoButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frameEvoUpgrade, framePrice, arrow);
            else UIHandler.instance.WeaponButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frame, framePrice, textLv, textDamageL, textDamageR, boxes, arrow);
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
        evoUpgrade.SetActive(false);
    }
}
