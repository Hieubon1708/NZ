using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class WeaponShoter : MonoBehaviour
{
    public GameController.WEAPON weaponType;
    public int level;
    public Animator ani;
    public Transform target;
    public Transform parent;
    protected float distance;
    public SortingGroup sortingGroup;
    public Animation scaleUpgrade;
    protected Coroutine rotate;
    Quaternion targetRotation;

    public virtual void StartGame()
    {
        ani.SetBool("startGame", true);
    }

    public abstract void UseBooster();
    public abstract void DisableWeapon();
    public abstract void SetDamageBooster(int damage);
    public abstract void SetDamage(int damage);

    public virtual void Restart()
    {
        ani.Rebind();
        if (parent != null) parent.localRotation = Quaternion.identity;
        if (rotate != null) StopCoroutine(rotate);
        target = null;
    }

    bool isNearest;

    protected void FindTarget()
    {
        Vector2 targetDir = Vector2.zero;
        if (GameController.instance.listEVisible.Count == 0) target = GameController.instance.defaultDir;
        else
        {
            isNearest = true;
            target = GameController.instance.listEVisible[Random.Range(0, GameController.instance.listEVisible.Count)].transform;
            if (weaponType == GameController.WEAPON.FLAME)
            {
                List<Transform> esByDistance = new List<Transform>();
                GameController.instance.GetEsByDistance(distance, parent.transform.position, esByDistance);
                if (esByDistance.Count != 0) target = esByDistance[Random.Range(0, esByDistance.Count)];
                else isNearest = false;
            }
            if (target.CompareTag("Enemy"))
            {
                targetDir = EnemyTowerController.instance.GetScE(target.gameObject).colObj.transform.position;
            }
        }
        targetDir = target.position;
        Vector2 direction = targetDir - (Vector2)parent.position;
        float angle = EUtils.GetAngle(direction);
        if(Vector2.Distance(transform.position, CarController.instance.transform.position) <= 1.5f) angle = Mathf.Clamp(angle, -7f, 180f);
        targetRotation = Quaternion.Euler(0, 0, angle);
        rotate = StartCoroutine(Rotate());
    }

    public abstract void LoadData();

    public IEnumerator Rotate()
    {
        while (true)
        {
            parent.localRotation = Quaternion.Lerp(parent.localRotation, targetRotation, 0.05f);
            yield return new WaitForFixedUpdate();
            if (!GameController.instance.listEVisible.Contains(target.gameObject) && target != GameController.instance.defaultDir) break;
            if (Quaternion.Angle(parent.localRotation, targetRotation) <= (target == GameController.instance.defaultDir ? 5f : 1f)) break;
        }
        yield return new WaitForSeconds(isNearest ? 0.5f : 0.1f);
        FindTarget();
    }
}
