using DG.Tweening;
using System.Collections;
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

    public void Start()
    {
        esmission = flameSmokeParticle.emission;
        esmissionChild = flameSmokeParticleChild.emission;
    }

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
        AttackCooldownChange();
        AttackDurationChange();
        SetBurning();
        Booster.instance.DecreaseEnergyFlame(level);
    }

    public void SetBurning()
    {
        if (instance.flames.Contains(FLAMEEVO.BURNING))
        {
            for (int i = 0; i < ParController.instance.flameThrowers.Length; i++)
            {
                ParController.instance.flameThrowers[i].name = "50";
            }
        }
    }

    public void AttackRadiusChange()
    {
        if (instance.IsFlameContains(FLAMEEVO.ATTACKRADIUS, level))
        {
            int amout = instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKRADIUS, level);
            float multiplier = 1;

            if (amout == 1) multiplier = 1.3f;
            else if (amout == 2) multiplier = 1.6f;
            else if (amout == 3) multiplier = 1.9f;

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
        if (instance.IsFlameContains(FLAMEEVO.ATTACKDURATION, level))
        {
            int amout = instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKDURATION, level);

            if (amout == 1) multiplier = 1.7f;
            else if (amout == 2) multiplier = 1.5f;
            else if (amout == 3) multiplier = 1.3f;
        }
        attackDuration = startAttackDuration * multiplier;
    }

    public void AttackCooldownChange()
    {
        float multiplier = 1;
        if (instance.IsFlameContains(FLAMEEVO.ATTACKCOOLDOWN, level))
        {
            int amout = instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKCOOLDOWN, level);

            if (amout == 1) multiplier = 0.75f;
            else if (amout == 2) multiplier = 0.5f;
            else if (amout == 3) multiplier = 0.25f;
        }
        cooldown = startCooldown * multiplier;
    }

    public override void StartGame()
    {
        shot = StartCoroutine(Shot());
        FindTarget();
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
        distance = 5f;
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
        distance = 10f;
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

        colBooster.name = ((int)(damage * multiplier)).ToString();
    }

    public override void SetDamage(int damage)
    {
        float multiplier = 1;

        IncreaseDamage(ref multiplier);

        col.name = ((int)(damage * multiplier)).ToString();
    }

    void IncreaseDamage(ref float multiplier)
    {
        if (instance.IsFlameContains(FLAMEEVO.INCREASEDAMAGE, level))
        {
            int amout = instance.GetAmoutFlameEvo(FLAMEEVO.INCREASEDAMAGE, level);

            if (amout == 1) multiplier = 1.2f;
            else if (amout == 2) multiplier = 1.4f;
            else if (amout == 3) multiplier = 1.6f;
        }
    }

    public override void DisableWeapon()
    {
        if (shot != null) StopCoroutine(shot);
        if (rotate != null) StopCoroutine(rotate);
        ShotHandle(false, 1f, 0.35f);
        colBooster.SetActive(false);
    }
}
