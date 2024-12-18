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
    FlameBooster booster;

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
        booster = Booster.instance.weaponBoosters[2] as FlameBooster;
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
        Booster.instance.DecreaseEnergyFlame(level);
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
        base.StartGame();
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

    public void FlameOnceParticle()
    {
        flameOnceParticle.Play();
    }

    public override void UseBooster()
    {
        AudioController.instance.PlaySoundWeapon2(AudioController.instance.flameBooster, 0.25f);
        ani.SetTrigger("booster");
    }

    void EndBooster()
    {
        shot = StartCoroutine(Shot());
    }

    void FadeOut()
    {
        AudioController.instance.StopSoundWeapon(AudioController.instance.weapon2, 0.25f);
        booster.isUseBooster = false;
        booster.CheckBooterState();
        colBooster.SetActive(false);
        FlameThrover(0f, 0.35f);
    }

    IEnumerator Shot()
    {
        distance = 5f;
        AudioController.instance.PlaySoundWeapon2(AudioController.instance.flame, 0.25f);
        while (true)
        {
            ShotHandle(true, 1f, 0.35f);
            yield return new WaitForSeconds(attackDuration);
            ShotHandle(false, 0f, 0.35f);
            AudioController.instance.StopSoundWeapon(AudioController.instance.weapon2, 0.25f);
            yield return new WaitForSeconds(cooldown);
            AudioController.instance.PlaySoundWeapon2(AudioController.instance.flame, 0.25f);
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

        colBooster.name = ((int)(damage * multiplier)).ToString() + " " + GetMultiplierBurning();
    }

    int GetMultiplierBurning()
    {
        if (instance.IsFlameContains(FLAMEEVO.BURNING, level))
        {
            int amout = instance.GetAmoutFlameEvo(FLAMEEVO.BURNING, level);
            if (amout == 1) return 1;
            else if (amout == 2) return 2;
            else if (amout == 3) return 3;
        }
        return 0;
    }

    public override void SetDamage(int damage)
    {
        float multiplier = 1;

        IncreaseDamage(ref multiplier);

        col.name = ((int)(damage * multiplier)).ToString() + " " + GetMultiplierBurning();
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
        ani.Rebind();
        AudioController.instance.StopSoundWeapon(AudioController.instance.weapon2, 0f);
        ani.SetBool("startGame", true);
        if (shot != null) StopCoroutine(shot);
        if (rotate != null) StopCoroutine(rotate);
        ShotHandle(false, 0f, 0.35f);
        colBooster.SetActive(false);
    }
}
