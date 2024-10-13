using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UpgradeEvolutionController;

public class Booster : MonoBehaviour
{
    public static Booster instance;

    public GameObject energy;
    public GameObject energyAds;
    public GameObject boom;
    public Image energyBar;
    public int amoutEnergy;
    public TextMeshProUGUI textAmoutEnergy;
    public GameObject sawBooster;
    public GameObject flameBooster;
    public GameObject machineGunBooster;
    public WeaponBooster[] weaponBoosters;

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        EnableBooster();
        CheckBoosterState();
        DoFill();
    }

    void DoFill()
    {
        float time = DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetSecondsUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level) : DataManager.instance.energyConfig.startSeconds;
        energyBar.DOFillAmount(1, 1f / time).SetEase(Ease.Linear).OnComplete(delegate
        {
            amoutEnergy++;
            energyBar.fillAmount = 0;
            CheckBoosterState();
            DoFill();
        });
    }

    public void CheckBoosterState()
    {
        for (int i = 0; i < weaponBoosters.Length; i++)
        {
            weaponBoosters[i].CheckBooterState();
            weaponBoosters[i].UpdateTextEnergy();
        }
        textAmoutEnergy.text = amoutEnergy.ToString();
    }

    public void SetActiveBooster(bool isActive)
    {
        energyAds.SetActive(isActive);
        energy.SetActive(isActive);
        boom.SetActive(isActive);
    }

    public void ResetBooster()
    {
        if (sawBooster.activeSelf) sawBooster.SetActive(false);
        if (flameBooster.activeSelf) flameBooster.SetActive(false);
        if (machineGunBooster.activeSelf) machineGunBooster.SetActive(false);
    }

    void EnableBooster()
    {
        for (int i = 0; i < BlockController.instance.tempBlocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.tempBlocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == GameController.WEAPON.SAW) sawBooster.SetActive(true);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == GameController.WEAPON.FLAME) flameBooster.SetActive(true);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == GameController.WEAPON.MACHINE_GUN) machineGunBooster.SetActive(true);
        }
    }

    public void CheckButtonState(GameController.WEAPON type)
    {
        bool isRemaining = false;
        for (int i = 0; i < BlockController.instance.tempBlocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.tempBlocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == type)
            {
                isRemaining = true;
            }
        }
        if(!isRemaining)
        {
            if (type == GameController.WEAPON.SAW) sawBooster.SetActive(false);
            if (type == GameController.WEAPON.FLAME) flameBooster.SetActive(false);
            if (type == GameController.WEAPON.MACHINE_GUN) machineGunBooster.SetActive(false);
        }
    }

    public void DecreaseEnergySaw()
    {
        if (UpgradeEvolutionController.instance.saws.Contains(SAWEVO.DECREASEENERGY))
        {
            for (int i = 0; i < weaponBoosters.Length; i++)
            {
                if (weaponBoosters[i] is SawBooster)
                {
                    weaponBoosters[i].SubtractEnergy(25f);
                }
            }
        }
    }

    public void DecreaseEnergyFlame()
    {
        if (UpgradeEvolutionController.instance.flames.Contains(FLAMEEVO.DECREASEENERGY))
        {
            int level = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.DECREASEENERGY);
            int percentage = 0;

            if (level == 1) percentage = 15;
            else if (level == 2) percentage = 30;

            for (int i = 0; i < weaponBoosters.Length; i++)
            {
                if (weaponBoosters[i] is FlameBooster)
                {
                    weaponBoosters[i].SubtractEnergy(percentage);
                }
            }
        }
    }

    public void DecreaseEnergyMachineGun()
    {
        if (UpgradeEvolutionController.instance.machineGuns.Contains(MACHINEGUNEVO.DECREASEENERGY))
        {
            int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.DECREASEENERGY);
            int percentage = 0;

            if (level == 1) percentage = 10;
            else if (level == 2) percentage = 20;

            for (int i = 0; i < weaponBoosters.Length; i++)
            {
                if (weaponBoosters[i] is MachineGunBooster)
                {
                    weaponBoosters[i].SubtractEnergy(percentage);
                }
            }
        }
    }

    private void OnDisable()
    {
        energyBar.DOKill();
        energyBar.fillAmount = 0;
    }
}
