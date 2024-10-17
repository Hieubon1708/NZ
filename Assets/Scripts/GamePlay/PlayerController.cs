using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Player player;
    public PlayerHandler playerHandler;

    public GameObject traectory;
    public Transform gunPivot;
    public Transform gunPivotForAni;
    public Animator playerAni;
    public ParticleSystem bulletPar;
    public GameObject boomPref;
    public Rigidbody2D[] listBooms;
    public SpriteRenderer[] boomSpriteRenderers;
    public Transform startBoom;
    public CapsuleCollider2D col;
    public Transform target;

    int boomIndex;
    public int boomCount;
    public bool isFindingTarget;
    bool isMouseDown;

    private void Awake()
    {
        instance = this;
        BoomGenerate();
    }

    public void Start()
    {
        SetParentForBoom();
    }

    public IEnumerator StartFindTarget()
    {
        yield return new WaitForFixedUpdate();
        target = GameController.instance.GetENearest(transform.position);
        isFindingTarget = true;
        ShotAni();
    }

    public void Restart()
    {
        playerAni.SetBool("attack", false);
        playerHandler.boxCollider.SetActive(true);
        BulletController.instance.EndShot();
    }

    public void LoadData()
    {
        BoomChange();
        BoomSetDamage(EquipmentController.instance.GetEquipValue(EquipmentController.EQUIPMENTTYPE.GRENADE, EquipmentController.instance.playerInventory.boomLevel, EquipmentController.instance.playerInventory.boomLevelUpgrade));
        player.LoadData();
        player.playerSkiner.LoadData();
        playerHandler.LoadData();
        BulletController.instance.LoadData();
    }

    public void FindTarget()
    {
        if (isMouseDown) return;
        if (GameController.instance.listEVisible.Contains(EnemyTowerController.instance.GetTower().col))
        {
            target = EnemyTowerController.instance.GetTower().col.transform;
        }
        else
        {
            target = GameController.instance.GetENearest(transform.position);
        }
        isFindingTarget = true;
    }

    void BoomGenerate()
    {
        listBooms = new Rigidbody2D[boomCount];
        boomSpriteRenderers = new SpriteRenderer[boomCount];
        for (int i = 0; i < listBooms.Length; i++)
        {
            GameObject b = Instantiate(boomPref, transform);
            listBooms[i] = b.GetComponent<Rigidbody2D>();
            boomSpriteRenderers[i] = b.GetComponent<SpriteRenderer>();
            b.SetActive(false);
        }
    }

    public void BoomSetDamage(int damage)
    {
        for (int i = 0; i < listBooms.Length; i++)
        {
            listBooms[i].name = damage.ToString();
        }
    }

    public void BoomChange()
    {
        for (int i = 0; i < boomSpriteRenderers.Length; i++)
        {
            boomSpriteRenderers[i].sprite = player.playerSkiner.booms[EquipmentController.instance.playerInventory.boomLevel];
        }
    }

    void SetParentForBoom()
    {
        for (int i = 0; i < listBooms.Length; i++)
        {
            listBooms[i].transform.SetParent(GameController.instance.poolDynamics);
        }
    }

    public void ThrowBoom()
    {
        GameObject b = listBooms[boomIndex].gameObject;
        Rigidbody2D rb = listBooms[boomIndex];
        b.SetActive(true);
        b.transform.position = startBoom.position;

        float bgSpeed = GameController.instance.backgroundSpeed * 2f;
        float blockHeight = BlockController.instance.tempBlocks.Count == 0 ? 0 : (transform.position.y - BlockController.instance.tempBlocks[0].transform.position.y) * 0.1f;

        rb.AddForce(new Vector2(Random.Range(2.75f + bgSpeed - blockHeight, 3.55f + bgSpeed - blockHeight), 5), ForceMode2D.Impulse);
        rb.AddTorque(0.75f, ForceMode2D.Impulse);
        boomIndex++;
        if (boomIndex == listBooms.Length) boomIndex = 0;
    }

    public void MouseDown()
    {
        isMouseDown = true;
        traectory.SetActive(true);
        isFindingTarget = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ThrowBoom();
        }
        if (isMouseDown)
        {
            Vector2 mousePos = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - (Vector2)gunPivot.position;
            float angle = Mathf.Clamp(Mathf.DeltaAngle(0f, EUtils.GetAngle(dir) - gunPivotForAni.localEulerAngles.z), -90f, 90f);
            gunPivot.localRotation = Quaternion.Euler(0, 0, angle);
        }
        if (isFindingTarget)
        {
            Vector2 dir = target.position - gunPivot.position;
            float angle = Mathf.Clamp(Mathf.DeltaAngle(0f, EUtils.GetAngle(dir) - gunPivotForAni.localEulerAngles.z), -90f, 90f);
            gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation, Quaternion.Euler(0, 0, angle), 0.1f);
        }
    }

    public void MouseUp()
    {
        if (!isMouseDown) return;
        isMouseDown = false;
        traectory.SetActive(false);
    }

    public void AddBookAni()
    {
        playerAni.SetTrigger("addBlock");
    }

    public void DeleteBookAni()
    {
        playerAni.SetTrigger("removeGameBlock");
    }

    public void ShotAni()
    {
        playerAni.SetBool("attack", true);
    }

    public void DeathAni()
    {
        playerAni.SetTrigger("death");
    }
}
