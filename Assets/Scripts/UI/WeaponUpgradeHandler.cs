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
    public GameObject boxBgs;
    public TextMeshProUGUI textMax;
    public TextMeshProUGUI textEvoUpgrade;
    public GameObject evoUpgrade;
    public GameObject[] boxProgress;
    public BlockUpgradeHandler blockUpgradeHandler;
    public WeaponShoter weaponShoter;
    public WeaponConfig weaponConfig;
    public TutorialOject tutorialOject;
    public Animation aniShowNewEvo;

    public void LoadData(int level, int levelUpgrade)
    {
        this.levelUpgrade = levelUpgrade;
        this.level = level;
    }

    public TutorialOject GetTutorialOject(out int price)
    {
        price = 0;
        if (evoUpgrade.activeSelf)
        {
            price = DataManager.instance.GetUpgradePriceWeaponConfig(level, levelUpgrade, weaponConfig);
            return tutorialOject;
        }
        else return null;
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

    public void EvoAccept()
    {
        blockUpgradeHandler.BuyWeapon(weaponShoter.weaponType, level);
        weaponShoter.LoadData();
    }

    public override void Upgrade()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        blockUpgradeHandler.blockInfo.PlusGold(DataManager.instance.GetUpgradePriceWeaponConfig(level, levelUpgrade, weaponConfig));
        levelUpgrade++;
        if (evoUpgrade.activeSelf)
        {
            level++;
            levelUpgrade = 0;
            UpgradeEvolutionController.instance.weaponUpgradeHandler = this;
            UIHandler.instance.tutorial.TutorialUpgradeWeaponEvo(true);
            evoUpgrade.SetActive(false);
            bool isEvoContains = false;

            if (weaponShoter.weaponType == GameController.WEAPON.SAW)
            {
                if (level > UpgradeEvolutionController.instance.saws.Count)
                {
                    UpgradeEvolutionController.instance.uIUpgradeEvolution.ShowPanelSawEvo(); isEvoContains = true;
                }
            }
            if (weaponShoter.weaponType == GameController.WEAPON.FLAME)
            {
                if (level > UpgradeEvolutionController.instance.flames.Count)
                {
                    UpgradeEvolutionController.instance.uIUpgradeEvolution.ShowPanelFlameEvo(); isEvoContains = true;
                }
            }
            if (weaponShoter.weaponType == GameController.WEAPON.MACHINE_GUN)
            {
                if (level > UpgradeEvolutionController.instance.machineGuns.Count)
                {
                    UpgradeEvolutionController.instance.uIUpgradeEvolution.ShowPanelMachineGunEvo(); isEvoContains = true;
                }
            }
            if (weaponShoter.weaponType == GameController.WEAPON.SHOCKER)
            {
                if (level > UpgradeEvolutionController.instance.shockers.Count)
                {
                    UpgradeEvolutionController.instance.uIUpgradeEvolution.ShowPanelShockerEvo(); isEvoContains = true;
                }
            }
            if (isEvoContains)
            {
                gameObject.SetActive(false);
            }
            else
            {
                EvoAccept();
            }
        }
        if(weaponShoter.scaleUpgrade.isPlaying) weaponShoter.scaleUpgrade.Stop();
        weaponShoter.scaleUpgrade.Play();
        UpgradeHandle();
        BlockController.instance.CheckButtonStateAll();
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
        UIHandler.instance.tutorial.TutorialUpgradeWeaponEvo(false);
        textEvoUpgrade.text = "Lv" + (level + 1) + " > " + "Lv" + (level + 2) + " EVOLUTION";
        textPriceUpgrade.text = DataManager.instance.GetEvolutionPriceWeaponConfig(level, weaponConfig).ToString();
    }

    public override void CheckButtonState()
    {
        if (weaponShoter == null) return;
        if (level == DataManager.instance.GetLengthUpgradePriceWeaponConfig(level, weaponConfig) - 1 && levelUpgrade == boxProgress.Length)
        {
            UIHandler.instance.WeaponButtonChangeState(frame, textPriceUpgrade, textMax, framePrice, arrow);
            boxBgs.SetActive(false);
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

        if (!BlockController.instance.IsWeaponExistBlock(weaponShoter.weaponType))
        {
            if (weaponShoter.weaponType == GameController.WEAPON.SAW) UpgradeEvolutionController.instance.saws.Clear();
            else if (weaponShoter.weaponType == GameController.WEAPON.FLAME) UpgradeEvolutionController.instance.flames.Clear();
            else if (weaponShoter.weaponType == GameController.WEAPON.MACHINE_GUN) UpgradeEvolutionController.instance.machineGuns.Clear();
        }

        weaponShoter.parent.gameObject.SetActive(false);
        weaponShoter = null;
        weaponConfig = null;
        level = 0;
        levelUpgrade = 0;
        evoUpgrade.SetActive(false);
        boxBgs.SetActive(true);
        arrow.gameObject.SetActive(true);
        textMax.gameObject.SetActive(false);
    }
}
