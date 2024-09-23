using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FlameHandler : WeaponShoter
{
    public ParticleSystem flameSmokeParticle;
    public ParticleSystem flameBoosterSmokeParticle;
    public BoxCollider2D attackCollider;
    public SpriteRenderer flameThrover;
    Coroutine shot;
    Coroutine boosterShot;
    EmissionModule esmission;

    public override void StartGame()
    {
        shot = StartCoroutine(Shot());
        StartCoroutine(FindTarget());
        StartCoroutine(Rotate());
    }

    public override void UseBooster()
    {
        ani.SetTrigger("booster");
        boosterShot = StartCoroutine(Booster());
    }

    IEnumerator Booster()
    {
        if(shot != null) StopCoroutine(shot);
        if(boosterShot != null) StopCoroutine(boosterShot);
        ShotHandle(true, 0f, 0f);
        attackCollider.transform.localScale = Vector3.one * 2;
        ColNFlameThrover(true, 1f, 0.5f);
        yield return new WaitWhile(() => flameBoosterSmokeParticle.emission.enabled);
        ColNFlameThrover(false, 0f, 0.5f);
        yield return new WaitForSeconds(1f);
        shot = StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        attackCollider.transform.localScale = Vector3.one;
        esmission = flameSmokeParticle.emission;
        while (true)
        {
            ShotHandle(true, 1f, 0.5f);
            yield return new WaitForSeconds(2.5f);
            ShotHandle(false, 0f, 0.5f);
            yield return new WaitForSeconds(1f);
        }
    }

    void ShotHandle(bool isActive, float alpha, float duration)
    {
        ani.SetBool("attack", isActive);
        esmission.enabled = isActive;
        ColNFlameThrover(isActive, alpha, duration);
    }

    void ColNFlameThrover(bool isActive, float alpha, float duration)
    {
        attackCollider.enabled = isActive;
        if (flameThrover != null) flameThrover.DOFade(alpha, duration);
    }
}
