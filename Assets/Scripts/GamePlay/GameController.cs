using DG.Tweening;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int level;

    public List<GameObject> listEnemies;
    public List<GameObject> listEVisible = new List<GameObject>();

    public GameObject[] mapLevels;

    public Transform poolDamages;
    public Transform poolWeapons;
    public Transform poolBullets;
    public Transform poolEnemies;
    public Transform poolDynamics;
    public Transform poolPars;
    public Transform defaultDir;

    public float timeBlockNPlayerDamage;
    public float timeSawDamage;
    public float timeFlameDamage;
    public float timeFlameBurningDamage;
    public float backgroundSpeed;

    public GameObject menuCamera;
    public GameObject gameCamera;
    public GameObject buttonStart;
    public GameObject touchScreen;
    public GameObject colDisplay;

    public bool isStart;
    public Camera cam;

    public GameObject v;

    private void Awake()
    {
        instance = this;
        DOTween.SetTweensCapacity(200, 1000);
        Resize();
        GenerateColDisplay();
    }

    void GenerateColDisplay()
    {
        Vector2 sceenR = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height / 2));
        Instantiate(colDisplay, new Vector2(sceenR.x + gameCamera.transform.position.x, sceenR.y), Quaternion.identity, transform);
    }

    void MapGenerate(int index)
    {
        Instantiate(mapLevels[index], transform);
    }

    public void Start()
    {
        LoadData();
        MapGenerate(level);
        ChangeBlockSprites(level);
        ChangeCarSprites(level);

        PlayerInventory.instance.LoadData();
        PlayerHandler.instance.LoadData();
        UIHandler.instance.LoadData();
        BlockController.instance.LoadData();
        SummonEquipment.instance.LoadData();
        EquipmentController.instance.LoadData();
        UpgradeEvolutionController.instance.LoadData();

        //Instantiate(v, new Vector2(CarController.instance.transform.position.x + 7, CarController.instance.transform.position.x + 3), Quaternion.identity);
        //Instantiate(v, new Vector2(CarController.instance.transform.position.x + 2.5f, CarController.instance.transform.position.y + 7), Quaternion.identity);
    }

    void LoadData()
    {
        level = DataManager.instance.dataStorage != null ? DataManager.instance.dataStorage.level : 0;
    }

    public void EDeathAll(GameObject tower)
    {
        for (int i = 0; i < listEVisible.Count; i++)
        {
            if (listEVisible[i] != tower)
            {
                ParController.instance.PlayZomDieParticle(listEVisible[i].transform.position);
            }
        }
        listEVisible.Clear();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            level = Mathf.Clamp(++level, 0, 5);
            ChangeBlockSprites(level);
            ChangeCarSprites(level);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            level = Mathf.Clamp(++level, 0, 5);
            ChangeBlockSprites(level);
            ChangeCarSprites(level);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Restart();
        }
    }

    public void ChangeBlockSprites(int level)
    {
        DataManager.instance.SetBlockSprites(level);
        BlockController.instance.ResetBlockSprites();
    }

    public void ChangeCarSprites(int level)
    {
        Sprite[] carSprites = DataManager.instance.GetCarSprites(level);

        if (carSprites != null)
        {
            CarController.instance.tent_part_3.sprite = carSprites[0];
            CarController.instance.tent_part_2.sprite = carSprites[1];
            CarController.instance.tent_part_1.sprite = carSprites[2];
            CarController.instance.chassis.sprite = carSprites[3];
            CarController.instance.wheelLeft.sprite = carSprites[4];
            CarController.instance.wheelRight.sprite = carSprites[4];
            CarController.instance.shadow.sprite = carSprites[5];
        }
        else
        {
            Debug.LogError("!");
        }
    }

    public void Restart()
    {
        SetValue(false);
        EnemyTowerController.instance.Restart();
        BlockController.instance.Restart();
        Booster.instance.ResetBooster();
    }

    void Resize()
    {
        float defaultSize = cam.orthographicSize;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 10.8f / 19.2f;
        if (screenRatio < targetRatio)
        {
            float changeSize = targetRatio / screenRatio;
            cam.orthographicSize = defaultSize * changeSize;
        }
    }

    public void StartGame()
    {
        SetValue(true);
        BlockController.instance.StartGame();
        Booster.instance.StartGame();
        EnemyTowerController.instance.NextTower();
    }

    void SetValue(bool isActive)
    {
        menuCamera.SetActive(!isActive);
        gameCamera.SetActive(isActive);
        touchScreen.SetActive(isActive);
        buttonStart.SetActive(!isActive);
        isStart = isActive;
        Booster.instance.energyAds.SetActive(isActive);
        Booster.instance.energy.SetActive(isActive);
        Booster.instance.boom.SetActive(isActive);
        BlockUpgradeController.instance.recyleClose.SetActive(!isActive);
        BlockController.instance.SetActiveUI(!isActive);
        CarController.instance.multiplier = isActive ? 1 : 0;
    }

    public enum WEAPON
    {
        NONE, SAW, FLAME, MACHINE_GUN
    }


    public Transform GetENearest(Vector2 startPos)
    {
        if (listEVisible.Count == 0) return defaultDir;
        int index = -1;
        float min = float.MaxValue;
        for (int i = 0; i < listEVisible.Count; i++)
        {
            float distance = Vector2.Distance(startPos, listEVisible[i].transform.position);
            if (distance < min)
            {
                min = distance;
                index = i;
            }
        }
        return listEVisible[index].transform;
    }

    public void OnDestroy()
    {
        BlockDataStorage[] blockDataStorages = new BlockDataStorage[BlockController.instance.blocks.Count];

        for (int i = 0; i < BlockController.instance.blocks.Count; i++)
        {
            Block scBlock = BlockController.instance.blocks[i].GetComponent<Block>();

            int blockLevel = scBlock.level;
            int blockGold = scBlock.sellingPrice;

            WEAPON weaponType = WEAPON.NONE;

            if (scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) weaponType = scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType;

            int weaponLevel = scBlock.blockUpgradeHandler.weaponUpgradeHandler.level;
            int weaponUpgradeLevel = scBlock.blockUpgradeHandler.weaponUpgradeHandler.levelUpgrade;

            WeaponDataStorage weaponDataStorage = new WeaponDataStorage(weaponType, weaponLevel, weaponUpgradeLevel);
            blockDataStorages[i] = new BlockDataStorage(blockLevel, blockGold, weaponDataStorage);
        }

        EquipmentDataStorage[] equipmentConfigs = new EquipmentDataStorage[EquipmentController.instance.amoutEquip];

        for (int i = 0; i < equipmentConfigs.Length; i++)
        {
            equipmentConfigs[i] = new EquipmentDataStorage((int)EquipmentController.instance.equipments[i].type, (int)EquipmentController.instance.equipments[i].level);
        }

        DesignDataStorage designDataStorage = new DesignDataStorage(PlayerInventory.instance.amoutGunDesign, PlayerInventory.instance.amoutCapDesign, PlayerInventory.instance.amoutBoomDesign, PlayerInventory.instance.amoutClothesDesign);
        EquipmentUpgradeDataStorage equipmentUpgradeDataStorage = new EquipmentUpgradeDataStorage(PlayerInventory.instance.gunLevelUpgrade, PlayerInventory.instance.boomLevelUpgrade, PlayerInventory.instance.capLevelUpgrade, PlayerInventory.instance.clothesLevelUpgrade);
        PLayerDataStorage pLayerDataStorage = new PLayerDataStorage(PlayerHandler.instance.playerInfo.gold, PlayerInventory.instance.gunLevel, PlayerInventory.instance.boomLevel, PlayerInventory.instance.clothesLevel, PlayerInventory.instance.clothesLevel, equipmentConfigs, equipmentUpgradeDataStorage, designDataStorage);
        EnergyDataStorage energyDataStorage = new EnergyDataStorage(BlockController.instance.energyUpgradee.level);
        WeaponEvolutionDataStorge weaponEvolutionDataStorge = new WeaponEvolutionDataStorge(UpgradeEvolutionController.instance.saws.ToArray(), UpgradeEvolutionController.instance.flames.ToArray(), UpgradeEvolutionController.instance.machineGuns.ToArray());
        ChanceDataStorage chanceDataStorage = new ChanceDataStorage(SummonEquipment.instance.level, SummonEquipment.instance.amout);

        DataStorage dataStorage = new DataStorage(level, pLayerDataStorage, blockDataStorages, energyDataStorage, weaponEvolutionDataStorge, chanceDataStorage);

        string dataStorageJs = JsonConvert.SerializeObject(dataStorage);
        string path = Path.Combine(Application.persistentDataPath, "DataStorage.json");
        File.WriteAllText(path, dataStorageJs);
    }
}
