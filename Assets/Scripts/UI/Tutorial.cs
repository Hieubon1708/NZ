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
    public bool isSecondTimeDestroyTower;
    public bool isFirstTimeDragBlock;
    public bool isFirstTimeClickUpgradeWeaponEvo;

    public GameObject buttonBuyBlock;
    public GameObject buttonUpgradeEnergy;
    public GameObject buttonReward;
    public GameObject blockDragTutorial;

    public TutorialOject scButtonBuyBlock;
    public TutorialOject scButtonBuyWeapon;
    public TutorialOject scButtonUpgradeWeaponEvo;
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

    public void LoadData()
    {
        if (DataManager.instance.dataStorage.tutorialDataStorage != null)
        {
            isFirstTimePlay = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimePlay;

            isFirstTimeClickButtonBuyBlock = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickButtonBuyBlock;
            isFirstTimeClickButtonBuyWeapon = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickButtonBuyWeapon;
            isFirstTimeClickButtonUpgradeEnergy = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickButtonUpgradeEnergy;
            isFirstTimeClickUpgradeWeaponEvo = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickUpgradeWeaponEvo;

            isFirstTimeClickBoosterBoom = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickBoosterBoom;
            isFirstTimeClickBoosterSaw = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickBoosterSaw;
            isFirstTimeClickBoosterFlame = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickBoosterFlame;
            isFirstTimeClickBoosterMachineGun = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeClickBoosterMachineGun;

            isUnlockInventory = DataManager.instance.dataStorage.tutorialDataStorage.isUnlockInventory;
            isUnlockShop = DataManager.instance.dataStorage.tutorialDataStorage.isUnlockShop;
            isUnlockWeapon = DataManager.instance.dataStorage.tutorialDataStorage.isUnlockWeapon;
            isUnlockBoss = DataManager.instance.dataStorage.tutorialDataStorage.isUnlockBoss;

            isFirstTimeDestroyTower = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeDestroyTower;
            isSecondTimeDestroyTower = DataManager.instance.dataStorage.tutorialDataStorage.isSecondTimeDestroyTower;
            isFirstTimeDragBlock = DataManager.instance.dataStorage.tutorialDataStorage.isFirstTimeDragBlock;
        }
        if(isSecondTimeDestroyTower) UIHandler.instance.daily.daily.SetActive(true);
    }

    public TutorialDataStorage GetData()
    {
        return new TutorialDataStorage(isFirstTimePlay, isFirstTimeClickButtonBuyBlock, isFirstTimeClickButtonBuyWeapon
            , isFirstTimeClickButtonUpgradeEnergy, isFirstTimeClickUpgradeWeaponEvo, isFirstTimeClickBoosterBoom, isFirstTimeClickBoosterSaw
            , isFirstTimeClickBoosterFlame, isFirstTimeClickBoosterMachineGun, isFirstTimeDestroyTower, isSecondTimeDestroyTower, isFirstTimeDragBlock
            , isUnlockInventory, isUnlockShop, isUnlockWeapon, isUnlockBoss);
    }

    public void TutorialButtonBuyBlock(bool isClick)
    {
        if (isFirstTimeClickButtonBuyBlock || !isClick && PlayerController.instance.player.gold < DataManager.instance.blockConfig.startPrice)
        {
            TutorialButtonBuyWeapon(false);
        }
        else
        {
            if (isClick)
            {
                EnableTutorial(false, scButtonBuyBlock);
                isFirstTimeClickButtonBuyBlock = true;
                TutorialButtonBuyWeapon(false);
            }
            else
            {
                scSelected = scButtonBuyBlock;
                EnableTutorial(true, scButtonBuyBlock);
            }
        }
    }

    public void TutorialButtonBuyWeapon(bool isClick)
    {
        int price = DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.SAW);
        if (isFirstTimeClickButtonBuyWeapon || !isClick && PlayerController.instance.player.gold < price || BlockController.instance.blocks.Count == 0)
        {
            TutorialButtonUpgradeEnergy(false);
        }
        else
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.blocks[0]);
            SawBuyHandler sawBuyHandler = sc.blockUpgradeHandler.weaponBuyButtons[0] as SawBuyHandler;
            scButtonBuyWeapon = sawBuyHandler.tutorialOject;
            if (isClick)
            {
                EnableTutorial(false, scButtonBuyWeapon);
                isFirstTimeClickButtonBuyWeapon = true;
                TutorialButtonUpgradeEnergy(false);
            }
            else
            {
                scSelected = scButtonBuyWeapon;
                EnableTutorial(true, scButtonBuyWeapon);
            }
        }
    }

    public void TutorialUpgradeWeaponEvo(bool isClick)
    {
        if (isFirstTimeClickUpgradeWeaponEvo)
        {
            EnableTutorial(false, scSelected);
            return;
        }
        int price = 0;
        scButtonUpgradeWeaponEvo = BlockController.instance.GetTutorialWeaponEvo(out price);
        if (!isClick && PlayerController.instance.player.gold < price)
        {
            EnableTutorial(false, scSelected);
        }
        else
        {
            if (isClick)
            {
                EnableTutorial(false, scButtonUpgradeWeaponEvo);
                isFirstTimeClickUpgradeWeaponEvo = true;
                TutorialButtonUpgradeEnergy(false);
            }
            else
            {
                scSelected = scButtonUpgradeWeaponEvo;
                EnableTutorial(true, scButtonUpgradeWeaponEvo);
            }
        }
    }

    public void TutorialButtonUpgradeEnergy(bool isClick)
    {
        if(!scButtonUpgradeEnergy.hand.activeSelf && isClick)
        {
            isFirstTimeClickButtonUpgradeEnergy = true;
            return;
        }
        int price = DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetPriceUpgradeEnergyConfig(BlockController.instance.energyUpgradee.level) : DataManager.instance.energyConfig.startPrice;
        if (isFirstTimeClickButtonUpgradeEnergy || !isClick && PlayerController.instance.player.gold < price)
        {
            TutorialDragBlock(false);
        }
        else
        {
            if (PlayerController.instance.player.gold < price)
            {
                isFirstTimeClickButtonUpgradeEnergy = true;
                EnableTutorial(false, scButtonUpgradeEnergy);
            }
            else
            {
                scSelected = scButtonUpgradeEnergy;
                EnableTutorial(true, scButtonUpgradeEnergy);
            }
        }
    }

    public void TutorialDragBlock(bool isClick)
    {
        if (isFirstTimeDragBlock || BlockController.instance.blocks.Count < 4)
        {
            TutorialUpgradeWeaponEvo(false);
        }
        else
        {
            if (isClick)
            {
                isFirstTimeDragBlock = true;
                blockDragTutorial.SetActive(false);
            }
            else
            {
                Vector2 pos = BlockController.instance.blocks[2].transform.position;
                blockDragTutorial.transform.position = GameController.instance.cam.WorldToScreenPoint(pos);
                TutorialUpgradeWeaponEvo(false);
                blockDragTutorial.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        if (blockDragTutorial.activeSelf) blockDragTutorial.SetActive(false);
    }

    public void TutorialButtonBooserBoom(bool isEnable, WeaponBooster weaponBooster, bool isEnoughEnergy)
    {
        if (isFirstTimeClickBoosterBoom || !(weaponBooster is BoomBooster)) return;
        if (!isEnable && isEnoughEnergy) isFirstTimeClickBoosterBoom = true;
        EnableTutorial(isEnable, scButtonBoosterBoom);
    }

    public void TutorialButtonBooserSaw(bool isEnable, WeaponBooster weaponBooster, bool isEnoughEnergy)
    {
        if (isFirstTimeClickBoosterSaw || !(weaponBooster is SawBooster)) return;
        if (!isEnable && isEnoughEnergy) isFirstTimeClickBoosterSaw = true;
        EnableTutorial(isEnable, scButtonBoosterSaw);
    }

    public void TutorialButtonBooserFlame(bool isEnable, WeaponBooster weaponBooster, bool isEnoughEnergy)
    {
        if (isFirstTimeClickBoosterFlame || !(weaponBooster is FlameBooster)) return;
        if (!isEnable && isEnoughEnergy) isFirstTimeClickBoosterFlame = true;
        EnableTutorial(isEnable, scButtonBoosterFlame);
    }

    public void TutorialButtonBooserMachineGun(bool isEnable, WeaponBooster weaponBooster, bool isEnoughEnergy)
    {
        if (isFirstTimeClickBoosterMachineGun || !(weaponBooster is MachineGunBooster)) return;
        if (!isEnable && isEnoughEnergy) isFirstTimeClickBoosterMachineGun = true;
        EnableTutorial(isEnable, scButtonBoosterMachineGun);
    }

    public void EnableTutorial(bool isEnable, TutorialOject tutorialOject)
    {
        if (tutorialOject != null) tutorialOject.EnabledTutorial(isEnable, unmaskButton, unmaskHand, unmaskOther, unmaskParent, xUnMask, spButtonUnmask, spOtherUnmask);
    }

    public void XUnmask()
    {
        EnableTutorial(false, scSelected);
        if (scSelected == scButtonBuyBlock) isFirstTimeClickButtonBuyBlock = true;
        if (scSelected == scButtonBuyWeapon) isFirstTimeClickButtonBuyWeapon = true;
        if (scSelected == scButtonUpgradeEnergy) isFirstTimeClickButtonUpgradeEnergy = true;
        if (scSelected == scButtonUpgradeWeaponEvo) isFirstTimeClickUpgradeWeaponEvo = true;
    }

    public void Restart()
    {
        if (!isFirstTimeClickBoosterBoom) EnableTutorial(false, scButtonBoosterBoom);
        if (!isFirstTimeClickBoosterSaw) EnableTutorial(false, scButtonBoosterSaw);
        if (!isFirstTimeClickBoosterFlame) EnableTutorial(false, scButtonBoosterFlame);
        if (!isFirstTimeClickBoosterMachineGun) EnableTutorial(false, scButtonBoosterMachineGun);
    }
}
