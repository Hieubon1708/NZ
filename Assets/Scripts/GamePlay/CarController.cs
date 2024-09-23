using System.Collections;
using UnityEngine;
using UnityEngine.TerrainTools;

public class CarController : MonoBehaviour
{
    public static CarController instance;

    public Animator carAni;
    public bool[] isBump;
    public float backgroundSpeed;
    public int amoutCollison = 3;
    public Transform[] spawnY;
    public bool isStop;
    public int multiplier;

    public SpriteRenderer tent_part_3;
    public SpriteRenderer tent_part_2;
    public SpriteRenderer tent_part_1;
    public SpriteRenderer chassis;
    public SpriteRenderer wheelLeft;
    public SpriteRenderer wheelRight;
    public SpriteRenderer shadow;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        backgroundSpeed = GameController.instance.backgroundSpeed;
    }

    public void AddBookAni()
    {
        carAni.SetTrigger("addBlock");
    }

    public void DeleteMenuBookAni()
    {
        carAni.SetTrigger("removeMenuBlock");
    }

    public void DeleteGameBookAni()
    {
        carAni.SetTrigger("removeGameBlock");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) amoutCollison--;
    }

    public IEnumerator Bump(LayerMask layerBumping, LayerMask layerOrigin, GameObject colObj, GameObject droping, GameObject ePush)
    {
        ePush.layer = layerBumping;
        colObj.layer = layerBumping;
        yield return new WaitWhile(() => Mathf.Abs(droping.transform.position.y - ePush.transform.position.y) >= 0.5f && droping.activeSelf && ePush.activeSelf);
        ePush.layer = layerOrigin;
        colObj.layer = layerOrigin;
    }

    private void FixedUpdate()
    {
        GameController.instance.backgroundSpeed = Mathf.Lerp(GameController.instance.backgroundSpeed, multiplier * backgroundSpeed / 3 * Mathf.Clamp(amoutCollison, 0, 3), 0.1f);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) amoutCollison++;
    }
}
