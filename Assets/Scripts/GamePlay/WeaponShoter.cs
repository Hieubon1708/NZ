using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponShoter : MonoBehaviour
{
    public GameController.WEAPON weaponType;
    public int level;
    public Animator ani;
    public Transform target;
    public Transform parent;
    protected float distance;
    protected Coroutine rotate;
    Quaternion targetRotation;

    public abstract void StartGame();
    public abstract void UseBooster();
    public abstract void SetDamageBooster(int damage);
    public abstract void SetDamage(int damage);

    public virtual void Restart()
    {
        if (parent != null) parent.localRotation = Quaternion.identity;
        if (rotate != null) StopCoroutine(rotate);
        target = null;
    }

    protected void FindTarget()
    {
        if (GameController.instance.listEVisible.Count == 0) target = GameController.instance.defaultDir;
        else
        {
            target = GameController.instance.listEVisible[Random.Range(0, GameController.instance.listEVisible.Count)].transform;
            if (weaponType == GameController.WEAPON.FLAME)
            {
                List<Transform> esByDistance = new List<Transform>();
                GameController.instance.GetEsByDistance(distance, parent.transform.position, esByDistance);
                if(esByDistance.Count != 0) target = esByDistance[Random.Range(0, esByDistance.Count)];
            }
        }

        Vector3 direction = target.position - parent.position;
        targetRotation = Quaternion.Euler(0, 0, EUtils.GetAngle(direction));
        rotate = StartCoroutine(Rotate());
    }

    public abstract void LoadData();

    public IEnumerator Rotate()
    {
        while (true)
        {
            parent.localRotation = Quaternion.Lerp(parent.localRotation, targetRotation, 0.05f);
            yield return new WaitForFixedUpdate();
            if (Quaternion.Angle(parent.localRotation, targetRotation) <= (target == GameController.instance.defaultDir ? 5f : 1f)) break;
        }
        yield return new WaitForSeconds(0.5f);
        FindTarget();
    }
}
