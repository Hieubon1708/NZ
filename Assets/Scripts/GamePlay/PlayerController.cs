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

    void ThrowBoom()
    {
        GameObject b = listBooms[boomIndex].gameObject;
        Rigidbody2D rb = listBooms[boomIndex];
        b.SetActive(true);
        b.transform.position = startBoom.position;
        rb.isKinematic = false;
        rb.AddForce(new Vector2(Random.Range(3f, 3.5f), 7), ForceMode2D.Impulse);
        rb.AddTorque(0.5f, ForceMode2D.Impulse);
        DOVirtual.DelayedCall(1.5f, delegate
        {
            rb.isKinematic = true;
            b.SetActive(false);
        });
        boomIndex++;
        if(boomIndex == listBooms.Length) boomIndex = 0;
    }

    public void MouseDown()
    {
        //if (!GameManager.instance.isStart) return;
        /*isMouseDown = true;
        traectory.SetActive(true);*/
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ThrowBoom();
        }
        //if (!GameManager.instance.isStart) return;
        /*if (isMouseDown)
        {
            Vector2 mousePos = GameManager.instance.cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - (Vector2)gunPivot.position;
            float angle = Mathf.Clamp(Mathf.DeltaAngle(0f, GetAngle(dir) - gunPivotForAni.localEulerAngles.z), -90f, 90f);
            gunPivot.localRotation = Quaternion.Euler(0, 0, angle);
        }*/
    }

    public void MouseUp()
    {
        //if (!GameManager.instance.isStart) return;
        /*if (!isMouseDown) return;
        isMouseDown = false;
        traectory.SetActive(false);*/
    }

    float GetAngle(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
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
}
