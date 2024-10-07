using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UpgradeEvolutionController;

public class FlameHandler : WeaponShoter
{
    public ParticleSystem flameSmokeParticle;
    public ParticleSystem flameSmokeParticleChild;
    public ParticleSystem flameBoosterSmokeParticle;
    public ParticleSystem flameOnceParticle;
    public GameObject col;
    public GameObject colBooster;
    public SpriteRenderer flameThrover;
    public ParticleSystem[] flameEvos;
    public BoxCollider2D[] colEvos;
    public Vector2[] startColSizes;
    public float[] startSizes;

    Coroutine shot;
    EmissionModule esmission;
    EmissionModule esmissionChild;

    public float cooldown;
    public float startCooldown = 1;
    public float attackDuration;
    public float startAttackDuration = 2;

    public override void LoadData()
    {
        startColSizes = new Vector2[colEvos.Length];
        startSizes = new float[flameEvos.Length];

        for (int i = 0; i < startColSizes.Length; i++)
        {
            startColSizes[i] = colEvos[i].size;
        }
        for (int i = 0; i < startSizes.Length; i++)
        {
            startSizes[i] = flameEvos[i].main.startSize.constant;
        }

        AttackRadiusChange();
    }

    public void SetBurning()
    {
        if (instance.flames.Contains(FLAMEEVO.BURNING))
        {

        }
    }

    public void DecreaseEnergy()
    {
        if (instance.flames.Contains(FLAMEEVO.DECREASEENERGY))
        {
            int level = instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKRADIUS);
            int percentage = 0;

            if (level == 1) percentage = 15;
            else if (level == 2) percentage = 30;

            for (int i = 0; i < Booster.instance.weaponBoosters.Length; i++)
            {
                if (Booster.instance.weaponBoosters[i] is FlameBooster)
                {
                    Booster.instance.weaponBoosters[i].SubtractEnergy(percentage);
                }
            }
        }
    }

    public void AttackRadiusChange()
    {
        if (instance.flames.Contains(FLAMEEVO.ATTACKRADIUS))
        {
            int level = instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKRADIUS);
            float multiplier = 1;

            if (level == 1) multiplier = 1.3f;
            else if (level == 2) multiplier = 1.6f;
            else if (level == 3) multiplier = 1.9f;

            SetAttackRadius(multiplier);

            for (int i = 0; i < colEvos.Length; i++)
            {
                Vector2 newSize = new Vector2(startColSizes[i].x * multiplier, startColSizes[i].y * multiplier);
                colEvos[i].size = newSize;
            }
        }
    }

    public void AttackDurationChange()
    {
        float multiplier = 1;
        if (instance.flames.Contains(FLAMEEVO.ATTACKDURATION))
        {
            int level = instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKDURATION);

            if (level == 1) multiplier = 0.7f;
            else if (level == 2) multiplier = 0.5f;
            else if (level == 3) multiplier = 0.3f;
        }
        attackDuration = startAttackDuration * multiplier;
    }

    public void AttackCooldownChange()
    {
        float multiplier = 1;
        if (instance.flames.Contains(FLAMEEVO.ATTACKCOOLDOWN))
        {
            int level = instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKCOOLDOWN);

            if (level == 1) multiplier = 0.75f;
            else if (level == 2) multiplier = 0.5f;
            else if (level == 3) multiplier = 0.25f;
        }
        cooldown = startCooldown * multiplier;
    }

    public override void StartGame()
    {
        esmission = flameSmokeParticle.emission;
        esmissionChild = flameSmokeParticleChild.emission;
        shot = StartCoroutine(Shot());
        findTarget = StartCoroutine(FindTarget());
        rotate = StartCoroutine(Rotate());
    }

    public void SetAttackRadius(float multiplier)
    {
        for (int i = 0; i < flameEvos.Length; i++)
        {
            MainModule mainModule = flameEvos[i].main;
            mainModule.startSize = new MinMaxCurve(startSizes[i] * multiplier);
        }
    }

    public override void Restart()
    {
        base.Restart();
        if (shot != null) StopCoroutine(shot);
        ShotHandle(false, 0f, 0.0f);
        if (colBooster.activeSelf) colBooster.SetActive(false);
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
            yield return new WaitForSeconds(attackDuration);
            ShotHandle(false, 0f, 0.35f);
            yield return new WaitForSeconds(cooldown);
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
        float multiplier = 1;

        IncreaseDamage(ref multiplier);

        colBooster.name = (damage * multiplier).ToString();
    }

    public override void SetDamage(int damage)
    {
        float multiplier = 1;

        IncreaseDamage(ref multiplier);

        col.name = (damage * multiplier).ToString();
    }

    void IncreaseDamage(ref float multiplier)
    {
        if (instance.flames.Contains(FLAMEEVO.INCREASEDAMAGE))
        {
            int level = instance.GetAmoutFlameEvo(FLAMEEVO.INCREASEDAMAGE);

            if (level == 1) multiplier = 1.2f;
            else if (level == 2) multiplier = 1.4f;
            else if (level == 3) multiplier = 1.6f;
        }
    }
}
