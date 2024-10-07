using System.Collections;
using UnityEngine;

public abstract class WeaponShoter : MonoBehaviour
{
    public GameController.WEAPON weaponType;
    public Animator ani;
    public Transform target;
    public Transform parent;
    protected Coroutine rotate;
    protected Coroutine findTarget;

    public abstract void StartGame();
    public abstract void UseBooster();
    public abstract void SetDamageBooster(int damage);
    public abstract void SetDamage(int damage);

    public virtual void Restart()
    {
        if (parent != null) parent.localRotation = Quaternion.identity;
        if (rotate != null) StopCoroutine(rotate);
        if (findTarget != null) StopCoroutine(findTarget);
        target = null;
    }

    public abstract void LoadData();  

    public IEnumerator Rotate()
    {
        while (true)
        {
            if (target != null && GameController.instance.listEVisible.Contains(target.gameObject) || target == GameController.instance.defaultDir)
            {
                parent.localRotation = Quaternion.RotateTowards(Quaternion.Euler(0, 0, parent.localEulerAngles.z), Quaternion.Euler(0, 0, EUtils.GetAngle(target.position - parent.position)), 1.5f);
            }
            else
            {
                target = GameController.instance.GetENearest(parent.position);
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator FindTarget()
    {
        while (true)
        {
            target = GameController.instance.GetENearest(parent.position);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
