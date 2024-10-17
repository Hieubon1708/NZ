using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static CarController instance;

    public Animator carAni;
    public float backgroundSpeed;
    public float partSpeed;
    public int amoutCollison;
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
        partSpeed = backgroundSpeed / 3;
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
    
    public void DeathAni()
    {
        carAni.SetTrigger("death");
    }
    public GameObject i;
    public IEnumerator Bump(LayerMask layerBumping, LayerMask layerOrigin, GameObject colObj, GameObject droping, GameObject colDroping, GameObject ePush, EnemyHandler e, float distance)
    {
        i = droping;
        ePush.layer = layerBumping;
        colObj.layer = layerBumping;
        e.isBumping = true;
        yield return new WaitWhile(() => Mathf.Abs(droping.transform.position.y - ePush.transform.position.y) >= distance && ePush.activeSelf && colDroping.activeSelf);
        ePush.layer = layerOrigin;
        colObj.layer = layerOrigin;
        e.isBumping = false;
        if (e.a)
        {

        Debug.LogWarning(distance);
        Debug.LogWarning(Mathf.Abs(droping.transform.position.y - ePush.transform.position.y) >= distance);
        Debug.LogWarning(ePush.activeSelf);
        Debug.LogWarning(droping.activeSelf);
        }

    }

    private void FixedUpdate()
    {
        GameController.instance.backgroundSpeed = Mathf.Lerp(GameController.instance.backgroundSpeed, multiplier * (backgroundSpeed - (partSpeed * Mathf.Clamp(amoutCollison, 0, 3))), 0.1f);
    }
}
