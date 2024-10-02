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
        findTarget = StartCoroutine(FindTarget());
        rotate = StartCoroutine(Rotate());
    }

    public override void Restart()
    {
        base.Restart();
        if (shot != null) StopCoroutine(shot);
        ShotHandle(false, 0f, 0.0f);
        if(colBooster.activeSelf) colBooster.SetActive(false);
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
        colBooster.SetActive(false);
        FlameThrover(0f, 0.35f);
    }

    IEnumerator Shot()
    {
        while (true)
        {
            ShotHandle(true, 1f, 0.35f);
            yield return new WaitForSeconds(2.5f);
            ShotHandle(false, 0f, 0.35f);
            yield return new WaitForSeconds(1f);
        }
    }

    void StartBooster()
    {
        if (shot != null) StopCoroutine(shot);
        ShotHandle(false, 1f, 0.35f);
        colBooster.SetActive(true);
    }

    void ShotHandle(bool isActive, float alpha, float duration)
    {
        ani.SetBool("attack", isActive);
        esmission.enabled = isActive;
        esmissionChild.enabled = isActive;
        col.SetActive(isActive);
        FlameThrover(alpha, duration);
    }

    void FlameThrover(float alpha, float duration)
    {
        flameThrover.DOKill();
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
