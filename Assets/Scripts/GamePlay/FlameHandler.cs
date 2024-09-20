using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class FlameHandler : WeaponShoter
{
    public ParticleSystem flameSmokeParticle;
    public BoxCollider2D attackCollider;
    public SpriteRenderer flameThrover;

    public override void StartGame()
    {
        StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        EmissionModule esmission = flameSmokeParticle.emission;
        while (true)
        {
            esmission.enabled = true;
            attackCollider.enabled = true;
            if(flameThrover != null) flameThrover.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(2.5f);
            if (flameThrover != null) flameThrover.DOFade(0f, 0.5f);
            attackCollider.enabled = false; 
            esmission.enabled = false;
            yield return new WaitForSeconds(1f);
        }
    }
}
