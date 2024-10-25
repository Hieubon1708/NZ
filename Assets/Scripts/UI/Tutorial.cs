using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool isFirstTimePlay;

    public bool isFirstTimeClickButtonBuyBlock;
    public bool isFirstTimeClickButtonBuyWeapon;
    public bool isFirstTimeClickButtonUpgradeEnergy;

    public bool isFirstTimeClickBoosterBoom;
    public bool isFirstTimeClickBoosterSaw;
    public bool isFirstTimeClickBoosterFlame;
    public bool isFirstTimeClickBoosterMachineGun;

    public bool isUnlockInventory;
    public bool isUnlockShop;
    public bool isUnlockWeapon;
    public bool isUnlockBoss;

    public bool isFirstTimeDestroyTower;

    public GameObject buttonBuyBlock;
    public GameObject buttonUpgradeEnergy;
    public GameObject buttonReward;

    public TutorialOject scButtonBuyBlock;
    public TutorialOject scButtonBuyWeapon;
    public TutorialOject scButtonUpgradeEnergy;
    public TutorialOject scButtonBoosterBoom;
    public TutorialOject scButtonBoosterSaw;
    public TutorialOject scButtonBoosterFlame;
    public TutorialOject scButtonBoosterMachineGun;

    public Unmask unmaskButton;
    public Unmask unmaskHand;
    public Unmask unmaskOther;
    public GameObject unmaskParent;
    public Image xUnMask;
    public Image spButtonUnmask;
    public Image spOtherUnmask;

    public TutorialOject scSelected;

    private void Start()
    {
        buttonUpgradeEnergy.transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(CarController.instance.transform.position.x + 2f, CarController.instance.transform.transform.position.y - 1.25f));
        buttonReward.transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(CarController.instance.transform.position.x + 5.5f, CarController.instance.transform.transform.position.y - 1.25f));
        buttonBuyBlock.transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(CarController.instance.transform.position.x - 1.25f, CarController.instance.transform.transform.position.y - 1.25f));
    }

    public void TutorialButtonBuyBlock(bool isClick)
    {
        if (isFirstTimeClickButtonBuyBlock || PlayerController.instance.player.gold < DataManager.instance.blockConfig.startPrice)
        {
            EnableTutorial(false, scButtonBuyBlock);
            TutorialButtonBuyWeapon(false);
        }
        else
        {
            if(isClick) isFirstTimeClickButtonBuyBlock = true;
            scSelected = scButtonBuyBlock;
            EnableTutorial(true, scButtonBuyBlock);
        }
    }

    public void TutorialButtonBuyWeapon(bool isClick)
    {
        int price = DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW);
        if (isFirstTimeClickButtonBuyWeapon || PlayerController.instance.player.gold < price)
        {
            TutorialButtonUpgradeEnergy(false);
        }
        else
        {
            if (BlockController.instance.blocks.Count > 0)
            {
                Block sc = BlockController.instance.GetScBlock(BlockController.instance.blocks[0]);
                SawBuyHandler sawBuyHandler = sc.blockUpgradeHandler.weaponBuyButtons[0] as SawBuyHandler;
                scButtonBuyWeapon = sawBuyHandler.tutorialOject;
                if (isClick)
                {
                    EnableTutorial(false, scButtonBuyWeapon);
                    isFirstTimeClickButtonBuyWeapon = true;
                }
                else
                {
                    scSelected = scButtonBuyWeapon;
                    EnableTutorial(true, scButtonBuyWeapon);
                }
            }
        }
    }

    public void TutorialButtonUpgradeEnergy(bool isClick)
    {
        int price = DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetPriceUpgradeEnergyConfig(BlockController.instance.energyUpgradee.level) : DataManager.instance.energyConfig.startPrice;
        if (isFirstTimeClickButtonUpgradeEnergy || !isClick && PlayerController.instance.player.gold < price)
        {
            EnableTutorial(false, scSelected);
        }
        else
        {
            if(isClick) isFirstTimeClickButtonUpgradeEnergy = true;
            if (PlayerController.instance.player.gold < price)
            {
                EnableTutorial(false, scButtonUpgradeEnergy);
                TutorialButtonBuyWeapon(false);
            }
            else
            {
                scSelected = scButtonUpgradeEnergy;
                EnableTutorial(true, scButtonUpgradeEnergy);
            }
        }
    }

    public void TutorialButtonBooserBoom(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterBoom || !(weaponBooster is BoomBooster)) return;
        if (!isEnable) isFirstTimeClickBoosterBoom = true;
        EnableTutorial(isEnable, scButtonBoosterBoom);
    }

    public void TutorialButtonBooserSaw(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterSaw || !(weaponBooster is SawBooster)) return;
        if (!isEnable) isFirstTimeClickBoosterSaw = true;
        EnableTutorial(isEnable, scButtonBoosterSaw);
    }

    public void TutorialButtonBooserFlame(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterFlame || !(weaponBooster is FlameBooster)) return;
        if (!isEnable) isFirstTimeClickBoosterFlame = true;
        EnableTutorial(isEnable, scButtonBoosterFlame);
    }

    public void TutorialButtonBooserMachineGun(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterMachineGun || !(weaponBooster is MachineGunBooster)) return;
        if (!isEnable) isFirstTimeClickBoosterMachineGun = true;
        EnableTutorial(isEnable, scButtonBoosterMachineGun);
    }

    void EnableTutorial(bool isEnable, TutorialOject tutorialOject)
    {
        tutorialOject.EnabledTutorial(isEnable, unmaskButton, unmaskHand, unmaskOther, unmaskParent, xUnMask, spButtonUnmask, spOtherUnmask);
    }

    public void XUnmask()
    {
        EnableTutorial(false, scSelected);
        if (scSelected == scButtonBuyBlock) isFirstTimeClickButtonBuyBlock = true; 
        if (scSelected == scButtonBuyWeapon) isFirstTimeClickButtonBuyWeapon = true; 
        if (scSelected == scButtonUpgradeEnergy) isFirstTimeClickButtonUpgradeEnergy = true; 
    }
}
