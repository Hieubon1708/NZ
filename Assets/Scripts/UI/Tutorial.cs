using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public bool isFirstTimePlay;

    public bool isFirstTimeClickButtonBuyBlock;
    public bool isFirstTimeClickButtonBuyWeapon;

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
    public TutorialOject scButtonUpgradeEnergy;
    public TutorialOject scButtonBoosterBoom;
    public TutorialOject scButtonBoosterSaw;
    public TutorialOject scButtonBoosterFlame;
    public TutorialOject scButtonBoosterMachineGun;

    public Unmask unmask;
    public GameObject unmaskObj;

    private void Start()
    {
        buttonUpgradeEnergy.transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(CarController.instance.transform.position.x + 2f, CarController.instance.transform.transform.position.y - 1.25f));
        buttonReward.transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(CarController.instance.transform.position.x + 5.5f, CarController.instance.transform.transform.position.y - 1.25f));
        buttonBuyBlock.transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(CarController.instance.transform.position.x - 1.25f, CarController.instance.transform.transform.position.y - 1.25f));
    }

    public void TutorialButtonBuyBlock(bool isEnable)
    {
        scButtonBuyBlock.EnabledTutorial(isEnable, unmask, unmaskObj);
    }

    public void TutorialButtonUpgradeEnergy(bool isEnable)
    {
        scButtonUpgradeEnergy.EnabledTutorial(isEnable, unmask, unmaskObj);
    }
    
    public void TutorialButtonBooserBoom(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterBoom || !(weaponBooster is BoomBooster)) return;
        if (!isEnable) isFirstTimeClickBoosterBoom = true;
        scButtonBoosterBoom.EnabledTutorial(isEnable, unmask, unmaskObj);
    }
    
    public void TutorialButtonBooserSaw(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterSaw || !(weaponBooster is SawBooster)) return;
        if (!isEnable) isFirstTimeClickBoosterSaw = true;
        scButtonBoosterSaw.EnabledTutorial(isEnable, unmask, unmaskObj);
    }
    
    public void TutorialButtonBooserFlame(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterFlame || !(weaponBooster is FlameBooster)) return;
        if (!isEnable) isFirstTimeClickBoosterFlame = true;
        scButtonBoosterFlame.EnabledTutorial(isEnable, unmask, unmaskObj);
    }
    
    public void TutorialButtonBooserMachineGun(bool isEnable, WeaponBooster weaponBooster)
    {
        if (isFirstTimeClickBoosterMachineGun || !(weaponBooster is MachineGunBooster)) return;
        if(!isEnable) isFirstTimeClickBoosterMachineGun = true;
        scButtonBoosterMachineGun.EnabledTutorial(isEnable, unmask, unmaskObj);
    }
}
