using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHandler : MonoBehaviour
{
    public Animator ani;
    public Boss bossInfo;
    public BossDamage bossDamage;
    public GameObject content;
    public HealthHandler healthHandler;
    public HitEffect hitEffect;
    public SpriteRenderer[] fullBodies;
    public BoxCollider2D col;
    public TextMeshProUGUI textCurrentHp;
    float lastFrame;
    public float targetX;
    Coroutine attack;

    Dictionary<GameObject, Coroutine> sawTriggers = new Dictionary<GameObject, Coroutine>();
    Dictionary<GameObject, Coroutine> flameTriggers = new Dictionary<GameObject, Coroutine>();
    Dictionary<GameObject, Coroutine> shockerTriggers = new Dictionary<GameObject, Coroutine>();
    Dictionary<GameObject, Coroutine> flameBurningTriggers = new Dictionary<GameObject, Coroutine>();

    public void LoadData()
    {
        healthHandler.SetTotalHp(DataManager.instance.bossConfig.targetProgress[UIBoss.instance.level]);
        int hp = DataManager.instance.bossConfig.targetProgress[UIBoss.instance.level] - UIBoss.instance.progress;
        bossInfo.hp = hp;
        healthHandler.SubtractHp(hp);
        UpdateTextHp(hp);
        UIBoss.instance.UpdateTextInProgress();
        UIBoss.instance.UpdateFillProgress();
    }


    public void Restart()
    {
        ani.Rebind();
        bossInfo.transform.localPosition = new Vector2(12, bossInfo.transform.localPosition.y);
    }

    void UpdateTextHp(int hp)
    {
        textCurrentHp.text = UIHandler.instance.ConvertNumberAbbreviation(hp);
    }

    private void Start()
    {
        lastFrame = content.transform.position.x;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (bossInfo.hp == 0) return;
        int subtractHp;
        if (collision.CompareTag("Bullet"))
        {
            subtractHp = int.Parse(collision.name);
            collision.gameObject.SetActive(false);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("MachineGun"))
        {
            subtractHp = int.Parse(collision.name);
            collision.gameObject.SetActive(false);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("SawBooster"))
        {
            subtractHp = int.Parse(collision.attachedRigidbody.name);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("Saw"))
        {
            subtractHp = int.Parse(collision.name);
            if (!sawTriggers.ContainsKey(collision.gameObject))
            {
                sawTriggers.Add(collision.gameObject, null);
            }
            sawTriggers[collision.gameObject] = StartCoroutine(SawTriggerHandle(subtractHp));
        }
        if (collision.CompareTag("Shocker"))
        {
            subtractHp = int.Parse(collision.name);
            if (!shockerTriggers.ContainsKey(collision.gameObject))
            {
                shockerTriggers.Add(collision.gameObject, null);
            }
            shockerTriggers[collision.gameObject] = StartCoroutine(ShockerTriggerHandle(subtractHp));
        }
        if (collision.CompareTag("ShockerBooster"))
        {
            subtractHp = int.Parse(collision.attachedRigidbody.name);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("Flame"))
        {
            subtractHp = int.Parse(collision.name.Substring(0, collision.name.Length - 1));
            int level = int.Parse(collision.name.Substring(collision.name.Length - 1, 1));

            if (level != 0)
            {
                if (!flameBurningTriggers.ContainsKey(collision.gameObject))
                {
                    flameBurningTriggers.Add(collision.gameObject, null);
                }
                if (flameBurningTriggers[collision.gameObject] != null) StopCoroutine(flameBurningTriggers[collision.gameObject]);

                int damageBurning = (int)(int.Parse(BulletController.instance.listBullets[0].name) * (0.75f * level));
                flameBurningTriggers[collision.gameObject] = StartCoroutine(FlameBurningTriggerHandle(damageBurning, collision.gameObject));
            }

            if (!flameTriggers.ContainsKey(collision.gameObject))
            {
                flameTriggers.Add(collision.gameObject, null);
            }
            flameTriggers[collision.gameObject] = StartCoroutine(FlameTriggerHandle(subtractHp));
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Saw"))
        {
            if (sawTriggers.ContainsKey(collision.gameObject) && sawTriggers[collision.gameObject] != null)
            {
                StopCoroutine(sawTriggers[collision.gameObject]);
            }
        }
        if (collision.CompareTag("Flame"))
        {
            if (flameTriggers.ContainsKey(collision.gameObject) && flameTriggers[collision.gameObject] != null)
            {
                StopCoroutine(flameTriggers[collision.gameObject]);
            }
        }
        if (collision.CompareTag("Shocker"))
        {
            if (shockerTriggers.ContainsKey(collision.gameObject) && shockerTriggers[collision.gameObject] != null)
            {
                StopCoroutine(shockerTriggers[collision.gameObject]);
            }
        }
        if (bossInfo.hp == 0) return;
        if (collision.CompareTag("Boom")) SubtractHp(int.Parse(collision.attachedRigidbody.name));
    }

    public void End()
    {
        if (attack != null) StopCoroutine(attack);
        attack = null;
        bossDamage.damageTaken = 0;
    }

    void SubtractHp(int subtractHp)
    {
        if (bossInfo.hp == 0) return;
        int randomCrit = Random.Range(0, 100);
        if (randomCrit <= 2) subtractHp = (int)(subtractHp * 1.5f);
        int hp = bossInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        UpdateTextHp(hp);
        UIBoss.instance.progress += subtractHp;
        UIBoss.instance.UpdateTextInProgress();
        UIBoss.instance.UpdateFillProgress();       
        bossDamage.ShowDamage(subtractHp, col, randomCrit <= 2f);
        hitEffect.PlayHitEffect(fullBodies);
        if (hp == 0)
        {

        }
    }

    IEnumerator FlameBurningTriggerHandle(int subtractHp, GameObject flame)
    {
        float time = 5f;
        while (bossInfo.hp > 0 && time > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeFlameBurningDamage);
            time -= GameController.instance.timeFlameBurningDamage;
        }
        flameBurningTriggers.Remove(flame);
    }

    protected IEnumerator PlayerTriggerHandle(int subtractHp)
    {
        while (PlayerController.instance.player.hp > 0)
        {
            PlayerController.instance.playerHandler.SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeBlockNPlayerDamage);
        }
    }

    IEnumerator SawTriggerHandle(int subtractHp)
    {
        while (bossInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeSawDamage);
        }
    }

    IEnumerator ShockerTriggerHandle(int subtractHp)
    {
        while (bossInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeShockerDamage);
        }
    }

    IEnumerator FlameTriggerHandle(int subtractHp)
    {
        while (bossInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
        }
    }

    private void FixedUpdate()
    {
        if (!GameController.instance.isStart) return;
        ani.SetFloat("speedMultiplier", -(content.transform.position.x - lastFrame) * 10f);
        lastFrame = content.transform.position.x;
        if (bossInfo.transform.position.x <= targetX && attack == null)
        {
            CarController.instance.multiplier = 0;
            GameController.instance.listEVisible.Add(col.gameObject);
            attack = StartCoroutine(Attack());
            PlayerController.instance.StartGame();
            Booster.instance.DoFill();
            GameController.instance.touchScreen.SetActive(true);
            for (int i = 0; i < BlockController.instance.blocks.Count; i++)
            {
                Block scBlock = BlockController.instance.GetScBlock(BlockController.instance.blocks[i]);
                scBlock.blockUpgradeHandler.weaponUpgradeHandler.StartGame();
            }
        }
        if (bossInfo.transform.position.x > targetX) bossInfo.transform.Translate(-Vector2.right * Time.fixedDeltaTime * 3);
    }

    IEnumerator Attack()
    {
        int frame = 265;
        while (bossInfo.hp > 0)
        {
            if (frame == 265)
            {
                frame = 0;
                ani.SetTrigger("attack0");
            }
            yield return new WaitForEndOfFrame();
            frame++;
        }
    }
}
