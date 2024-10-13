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
            if (target != null && GameController.instance.listEVisible.Contains(target.gameObject))
            {
                float time = 0.1f;
                if (Vector2.Distance(parent.position, target.position) < 3) time = 0.05f;
                if (Vector2.Distance(parent.position, target.position) < 1) time = 0.01f;
                float angle = EUtils.GetAngle(target.position - parent.position);
                if (parent.transform.position == BlockController.instance.tempBlocks[0].transform.position)
                {
                    angle = Mathf.Clamp(angle, -10f, 180f);
                }
                parent.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, parent.localEulerAngles.z), Quaternion.Euler(0, 0, angle), time);
            }
            else
            {
                target = GameController.instance.GetENearest(parent.position);
                if (findTarget != null) StopCoroutine(findTarget);
                StartCoroutine(FindTarget());
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public IEnumerator FindTarget()
    {
        while (true)
        {
            target = GameController.instance.GetENearest(parent.position);
            yield return new WaitForSeconds(3f);
        }
    }
}
