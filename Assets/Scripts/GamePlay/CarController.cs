using DG.Tweening;
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

    int count;
    bool isDelayEnemyAttack;
    Tween delayCall;

    private void Awake()
    {
        instance = this;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !GameController.instance.isLose)
        {
            if (count == 0 || !isDelayEnemyAttack)
            {
                delayCall.Kill();
                isDelayEnemyAttack = true;
                AudioController.instance.PlaySoundEnemyAttack(AudioController.instance.eAttack, AudioController.instance.attack[GameController.instance.level], 0.25f);
                delayCall = DOVirtual.DelayedCall(Random.Range(1.5f, 2.5f), delegate
                {
                    isDelayEnemyAttack = false;
                });
            }
            count++;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !GameController.instance.isLose)
        {
            count--;
            if (count == 0) AudioController.instance.StopSoundEnemyAttack(AudioController.instance.eAttack, 0.25f);
        }
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

    public IEnumerator Bump(LayerMask layerBumping, LayerMask layerOrigin, GameObject colObj, GameObject droping, GameObject colDroping, GameObject ePush, EnemyHandler e, float distance)
    {
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

    public void Restart()
    {
        count = 0;
        carAni.Rebind();
    }
}
