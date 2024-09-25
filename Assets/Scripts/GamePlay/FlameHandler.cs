using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FlameHandler : WeaponShoter
{
    public ParticleSystem flameSmokeParticle;
    public ParticleSystem flameSmokeParticleChild;
    public ParticleSystem flameBoosterSmokeParticle;
    public ParticleSystem flameOnceParticle;
    public GameObject col;
    public GameObject colBooster;
    public SpriteRenderer flameThrover;
    Coroutine shot;
    EmissionModule esmission;
    EmissionModule esmissionChild;

    public override void StartGame()
    {
        esmission = flameSmokeParticle.emission;
        esmissionChild = flameSmokeParticleChild.emission;
        shot = StartCoroutine(Shot());
        StartCoroutine(FindTarget());
        StartCoroutine(Rotate());
    }

    public void FlameOnceParticle()
    {
        flameOnceParticle.Play();
    }

    public override void UseBooster()
    {
        ani.SetTrigger("booster");
    }

    void EndBooster()
    {
        shot = StartCoroutine(Shot());
    }

    void FadeOut()
    {
        ColNFlameThrover(false, 0f, 0.25f);
    }

    IEnumerator Shot()
    {
        col.SetActive(true);
        colBooster.SetActive(false);
        while (true)
        {
            ShotHandle(true, 1f, 0.25f);
            yield return new WaitForSeconds(2.5f);
            ShotHandle(false, 0f, 0.25f);
            yield return new WaitForSeconds(1f);
        }
    }

    void StartBooster()
    {
        if (shot != null) StopCoroutine(shot);
        esmission.enabled = false;
        esmissionChild.enabled = false;
        col.SetActive(false);
        colBooster.SetActive(true);
        ColNFlameThrover(true, 1f, 0.25f);
    }

    void ShotHandle(bool isActive, float alpha, float duration)
    {
        ani.SetBool("attack", isActive);
        esmission.enabled = isActive;
        esmissionChild.enabled = isActive;
        ColNFlameThrover(isActive, alpha, duration);
    }

    void ColNFlameThrover(bool isActive, float alpha, float duration)
    {
        flameThrover.DOKill();
        col.SetActive(isActive);
        if (flameThrover != null) flameThrover.DOFade(alpha, duration);
    }

    public override void SetDamageBooster(int damage)
    {
        colBooster.name = damage.ToString();
    }

    public override void SetDamage(int damage)
    {
        col.name = damage.ToString();
    }
}
