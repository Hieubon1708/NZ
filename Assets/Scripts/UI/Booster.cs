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
    public GameObject shockerBooster;
    public WeaponBooster[] weaponBoosters;
    public Sprite frameDelay;

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

    public void KillEnergyNBoosterButton()
    {
        energyBar.DOKill();
        ActiveBoosterButton(false);
    }

    public void PlusEnergy()
    {
        amoutEnergy += 12;
        textAmoutEnergy.text = amoutEnergy.ToString();
    }

    public void ActiveBoosterButton(bool isActive)
    {
        for (int i = 0; i < weaponBoosters.Length; i++)
        {
            weaponBoosters[i].ButtonActive(isActive);
        }
    }

    void DoFill()
    {
        float time = DataManager.instance.dataStorage.energyDataStorage != null ? DataManager.instance.GetSecondsUpgradeEnergyConfig(DataManager.instance.dataStorage.energyDataStorage.level) : DataManager.instance.energyConfig.startSeconds;
        energyBar.DOFillAmount(1, 1f).SetEase(Ease.Linear).OnComplete(delegate
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
        energyBar.fillAmount = 0;
        if (sawBooster.activeSelf) sawBooster.SetActive(false);
        if (flameBooster.activeSelf) flameBooster.SetActive(false);
        if (machineGunBooster.activeSelf) machineGunBooster.SetActive(false);
        if (shockerBooster.activeSelf) shockerBooster.SetActive(false);
        ActiveBoosterButton(true);
    }

    void EnableBooster()
    {
        for (int i = 0; i < BlockController.instance.tempBlocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.tempBlocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == GameController.WEAPON.SAW) sawBooster.SetActive(true);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == GameController.WEAPON.FLAME) flameBooster.SetActive(true);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == GameController.WEAPON.MACHINE_GUN) machineGunBooster.SetActive(true);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == GameController.WEAPON.SHOCKER) shockerBooster.SetActive(true);
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
        if (!isRemaining)
        {
            if (type == GameController.WEAPON.SAW) sawBooster.SetActive(false);
            if (type == GameController.WEAPON.FLAME) flameBooster.SetActive(false);
            if (type == GameController.WEAPON.MACHINE_GUN) machineGunBooster.SetActive(false);
            if (type == GameController.WEAPON.SHOCKER) shockerBooster.SetActive(false);
        }
    }

    public void DecreaseEnergySaw(int level)
    {
        if (UpgradeEvolutionController.instance.IsSawContains(SAWEVO.DECREASEENERGY, level))
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
    
    public void DecreaseEnergyShocker(int level)
    {
        if (UpgradeEvolutionController.instance.IsShockerContains(SHOCKEREVO.DECREASEENERGY, level))
        {
            for (int i = 0; i < weaponBoosters.Length; i++)
            {
                if (weaponBoosters[i] is ShockerBooster)
                {
                    weaponBoosters[i].SubtractEnergy(20f);
                }
            }
        }
    }

    public void DecreaseEnergyFlame(int level)
    {
        if (UpgradeEvolutionController.instance.IsFlameContains(FLAMEEVO.DECREASEENERGY, level))
        {
            int amout = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.DECREASEENERGY, UpgradeEvolutionController.instance.flames.Count);
            int percentage = 0;

            if (amout == 1) percentage = 15;
            else if (amout == 2) percentage = 30;

            for (int i = 0; i < weaponBoosters.Length; i++)
            {
                if (weaponBoosters[i] is FlameBooster)
                {
                    weaponBoosters[i].SubtractEnergy(percentage);
                }
            }
        }
    }

    public void DecreaseEnergyMachineGun(int level)
    {
        if (UpgradeEvolutionController.instance.IsMachineGunContains(MACHINEGUNEVO.DECREASEENERGY, level))
        {
            int amout = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.DECREASEENERGY, UpgradeEvolutionController.instance.machineGuns.Count);
            int percentage = 0;

            if (amout == 1) percentage = 10;
            else if (amout == 2) percentage = 20;

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
