using System.Collections;
using UnityEngine;

public abstract class WeaponShoter : MonoBehaviour
{
    public Animator ani;
    public Transform target;

    public abstract void StartGame();

    public IEnumerator Rotate()
    {
        while (true)
        {
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, transform.localEulerAngles.z), Quaternion.Euler(0, 0, EUtils.GetAngle(target.position - transform.position)), 0.05f);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator FindTarget()
    {
        while (true)
        {
            target = GameController.instance.GetENearest(transform.position);
            float time = target == GameController.instance.defaultDir ? 0.02f : 0.5f;
            yield return new WaitForSeconds(time);
        }
    }
}
