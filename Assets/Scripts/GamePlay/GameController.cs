using Cinemachine;
using DG.Tweening;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int level;

    public bool isLose;

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

    public float xPlus1, xPlus2;
    public float yPlus1, yPlus2;

    public float timeBlockNPlayerDamage;
    public float timeSawDamage;
    public float timeShockerDamage;
    public float timeFlameDamage;
    public float timeFlameBurningDamage;
    public float backgroundSpeed;

    public GameObject menuCamera;
    public GameObject gameCamera;
    public GameObject buttonStart;
    public GameObject touchScreen;
    public GameObject colDisplay;
    public GameObject menu;

    public bool isStart;
    public Camera cam;

    public GameObject v;

    private void Awake()
    {
        instance = this;
        DOTween.SetTweensCapacity(200, 1000);
        //Resize();
        GenerateColDisplay();
    }

    void GenerateColDisplay()
    {
        Vector2 sceenR = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height / 2));
        Instantiate(colDisplay, new Vector2(sceenR.x + gameCamera.transform.position.x, sceenR.y), Quaternion.identity, transform);
    }

    void MapGenerate(int index)
    {
        Instantiate(mapLevels[index], new Vector2(0, 0.85f), Quaternion.identity, transform);
        ChangeBlockSprites(level);
        ChangeCarSprites(level);
    }

    public void EndGame()
    {
        ShakeCam(0.25f);
        isLose = true;
        touchScreen.SetActive(false);
    }

    public void ShakeCam(float strength)
    {
        gameCamera.transform.DOComplete();
        gameCamera.transform.DOShakePosition(1f, strength);
    }

    public void GetEsByDistance(float distance, Vector2 from, List<Transform> es)
    {
        es.Clear();
        for (int i = 0; i < listEVisible.Count; i++)
        {
            if (Vector2.Distance(from, listEVisible[i].transform.position) <= distance) es.Add(listEVisible[i].transform);
        }
    }

    public void Start()
    {
        LoadData();
        MapGenerate(level);

        EquipmentController.instance.LoadData();
        PlayerController.instance.LoadData();
        UIHandler.instance.LoadData();
        UpgradeEvolutionController.instance.LoadData();
        BlockController.instance.LoadData();

       /* Instantiate(v, new Vector2(CarController.instance.transform.position.x + 4f, CarController.instance.transform.position.y + 3), Quaternion.identity);
        Instantiate(v, new Vector2(CarController.instance.transform.position.x + 8f, CarController.instance.transform.position.y + 3), Quaternion.identity);
*/
        if (!UIHandler.instance.tutorial.isFirstTimePlay)
        {
            StartGame();
        }
        else
        {
            UIHandler.instance.tutorial.TutorialButtonBuyBlock(false);
        }
    }

    public GameObject EBlockNearest(LayerMask layer)
    {
        float min = float.MaxValue;
        GameObject e = null;
        for (int i = 0; i < listEVisible.Count; i++)
        {
            if (listEVisible[i].layer == layer && listEVisible[i].transform.position.x < min)
            {
                min = listEVisible[i].transform.position.x;
                e = listEVisible[i];
            }
        }
        return e;
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
                EnemyHandler sc = EnemyTowerController.instance.GetScE(listEVisible[i]);
                sc.damage.ShowDamage(sc.enemyInfo.hp.ToString(), sc.hitObj);
            }
        }
        EnemyTowerController.instance.DisableEs();
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
        UIHandler.instance.Restart();
        PlayerController.instance.Restart();
        BlockController.instance.Restart();
        CarController.instance.Restart();
        Booster.instance.ResetBooster();
        ParController.instance.SetActivePar(false);
        listEVisible.Clear();
    }

    void Resize()
    {
        float defaultSize = cam.orthographicSize;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 10.8f / 19.2f;
        if (screenRatio >= targetRatio)
        {
            menuCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize;
            gameCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize;
        }
        else
        {
            float changeSize = targetRatio / screenRatio;
            menuCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize * changeSize;
            gameCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize * changeSize;
        }
    }

    public void StartGame()
    {
        SetValue(true);
        BlockController.instance.StartGame();
        Booster.instance.StartGame();
        UIHandler.instance.StartGame();
        EnemyTowerController.instance.NextTower();
        PlayerController.instance.StartGame();
    }

    void SetValue(bool isActive)
    {
        menuCamera.SetActive(!isActive);
        gameCamera.SetActive(isActive);
        touchScreen.SetActive(isActive);
        buttonStart.SetActive(!isActive);
        menu.SetActive(!isActive);
        isStart = isActive;
        Booster.instance.SetActiveBooster(isActive);
        BlockUpgradeController.instance.recyleClose.SetActive(!isActive);
        BlockController.instance.SetActiveUI(!isActive);
        UIHandler.instance.SetValue(isActive);
        CarController.instance.multiplier = isActive ? 1 : 0;
    }

    public enum WEAPON
    {
        NONE, SAW, FLAME, MACHINE_GUN, SHOCKER
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

        DesignDataStorage designDataStorage = new DesignDataStorage(EquipmentController.instance.playerInventory.amoutGunDesign, EquipmentController.instance.playerInventory.amoutCapDesign, EquipmentController.instance.playerInventory.amoutBoomDesign, EquipmentController.instance.playerInventory.amoutClothesDesign);
        EquipmentUpgradeDataStorage equipmentUpgradeDataStorage = new EquipmentUpgradeDataStorage(EquipmentController.instance.playerInventory.gunLevelUpgrade, EquipmentController.instance.playerInventory.boomLevelUpgrade, EquipmentController.instance.playerInventory.capLevelUpgrade, EquipmentController.instance.playerInventory.clothesLevelUpgrade);
        playerDataStorage playerDataStorage = new playerDataStorage(PlayerController.instance.player.gold, EquipmentController.instance.playerInventory.gem, EquipmentController.instance.playerInventory.dush, EquipmentController.instance.playerInventory.key, EquipmentController.instance.playerInventory.cogwheel, EquipmentController.instance.playerInventory.gunLevel, EquipmentController.instance.playerInventory.boomLevel, EquipmentController.instance.playerInventory.clothesLevel, EquipmentController.instance.playerInventory.clothesLevel, equipmentConfigs, equipmentUpgradeDataStorage, designDataStorage);
        EnergyDataStorage energyDataStorage = new EnergyDataStorage(BlockController.instance.energyUpgradee.level);
        WeaponEvolutionDataStorge weaponEvolutionDataStorge = new WeaponEvolutionDataStorge(UpgradeEvolutionController.instance.GetSAWEVOS(), UpgradeEvolutionController.instance.GetFLAMEVOS(), UpgradeEvolutionController.instance.GetMACHINEGUNEVOS(), UpgradeEvolutionController.instance.GetSHOCKEREVOS());
        ChanceDataStorage chanceDataStorage = new ChanceDataStorage(0, 0);
        TutorialDataStorage tutorialDataStorage = UIHandler.instance.tutorial.GetData();
        DailyDataStorage dailyDataStorage = UIHandler.instance.daily.GetData();

        DataStorage dataStorage = new DataStorage(level, UIHandler.instance.setting.isSoundActive, UIHandler.instance.setting.isMusicActive
            , UIHandler.instance.lastRewardTime, UIHandler.instance.goldRewardHighest
            , UIHandler.instance.progressHandler.progresses.ToArray(), dailyDataStorage, tutorialDataStorage
            , playerDataStorage, blockDataStorages, energyDataStorage, chanceDataStorage, weaponEvolutionDataStorge);

        string dataStorageJs = JsonConvert.SerializeObject(dataStorage);
        string path = Path.Combine(Application.persistentDataPath, "DataStorage.json");
        File.WriteAllText(path, dataStorageJs);
    }
}
