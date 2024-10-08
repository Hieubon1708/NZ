using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    bool isMouseDown;
    public GameObject traectory;
    public Transform gunPivot;
    public Transform gunPivotForAni;
    public Animator playerAni;
    public ParticleSystem bulletPar;
    public GameObject boomPref;
    public Rigidbody2D[] listBooms;
    public SpriteRenderer[] boomSpriteRenderers;
    int boomIndex;
    public int boomCount;
    public Transform startBoom;
    Transform target;
    public bool isFindingTarget;
    public CapsuleCollider2D col;

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
        while (target != GameController.instance.defaultDir)
        {
            target = GameController.instance.GetENearest(transform.position);
            yield return new WaitForFixedUpdate();
        }
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

    public void BoomSkinChange()
    {
        for (int i = 0;i < boomSpriteRenderers.Length; i++)
        {
            boomSpriteRenderers[i].sprite = PlayerHandler.instance.playerInfo.playerSkiner.booms[EquipmentController.instance.indexBoom];
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
        rb.AddForce(new Vector2(Random.Range(2f + (CarController.instance.multiplier * 2), 2.5f + (CarController.instance.multiplier * 2)), 7), ForceMode2D.Impulse);
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
