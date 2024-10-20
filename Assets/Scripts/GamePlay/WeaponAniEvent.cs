using UnityEngine;

public class WeaponAniEvent : MonoBehaviour
{
    public ParticleSystem boosterShocker;

    public void PlayerParBoosterShocker()
    {
        boosterShocker.Play();
    }
}
