using System.Collections;
using UnityEngine;

public abstract class WeaponShoter : MonoBehaviour
{
    public Animator ani;
    public Transform target;
    public Transform parent;

    public abstract void StartGame();
    public abstract void UseBooster();
    public abstract void SetDamageBooster(int damage);
    public abstract void SetDamage(int damage);

    public IEnumerator Rotate()
    {
        while (true)
        {
            parent.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, parent.localEulerAngles.z), Quaternion.Euler(0, 0, EUtils.GetAngle(target.position - parent.position)), 0.05f);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator FindTarget()
    {
        while (true)
        {
            target = GameController.instance.GetENearest(parent.position);
            float time = target == GameController.instance.defaultDir ? 0.02f : 0.5f;
            yield return new WaitForSeconds(time);
        }
    }
}
