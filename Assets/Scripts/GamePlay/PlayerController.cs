using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
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
    int boomIndex;
    public int boomCount;
    public Transform startBoom;

    private void Awake()
    {
        instance = this;
        BoomGenerate();
    }

    public void Start()
    {
        SetParentForBoom();
    }

    void BoomGenerate()
    {
        listBooms = new Rigidbody2D[boomCount];
        for (int i = 0; i < listBooms.Length; i++)
        {
            GameObject b = Instantiate(boomPref, transform);
            listBooms[i] = b.GetComponent<Rigidbody2D>();
            b.SetActive(false);
        }
    }

    void SetParentForBoom()
    {
        for (int i = 0; i < listBooms.Length; i++)
        {
            listBooms[i].transform.SetParent(GameController.instance.poolWeapons);
        }
    }

    public void ThrowBoom()
    {
        GameObject b = listBooms[boomIndex].gameObject;
        Rigidbody2D rb = listBooms[boomIndex];
        b.SetActive(true);
        b.transform.position = startBoom.position;
        rb.AddForce(new Vector2(Random.Range(2f, 2.5f), 7), ForceMode2D.Impulse);
        rb.AddTorque(0.75f, ForceMode2D.Impulse);
        boomIndex++;
        if (boomIndex == listBooms.Length) boomIndex = 0;
    }

    public void MouseDown()
    {
        isMouseDown = true;
        traectory.SetActive(true);
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
