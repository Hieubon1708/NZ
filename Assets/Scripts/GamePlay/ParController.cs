using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ParController : MonoBehaviour
{
    public static ParController instance;

    public GameObject roadBulletHolePrefab;
    public GameObject zomDiePrefab;
    public GameObject blockDestroyPrefab;
    public GameObject towerExplosionHolePrefab;
    public GameObject zomHitOnRoadPrefab;
    public GameObject zomHitOnHeroPrefab;
    public GameObject towerExplosionPrefab;
    public GameObject flameThrowerPrefab;
    public GameObject stunOnEnemyPrefab;
    public GameObject playerDiePrefab;
    public GameObject boomEffectPrefab;
    public GameObject boomHolePrefab;
    public GameObject gunHitOnRoadPrefab;
    public GameObject gunHitOnEnemyPrefab;
    public GameObject gunHitExplosionPrefab;
    public GameObject[] gunHitOnRoads;
    public GameObject[] flameThrowers;
    public GameObject[] stunOnEnemies;
    public GameObject[] gunHitExplosions;
    public GameObject[] gunHitOnEnemies;
    public GameObject[] roadBulletHole;
    public GameObject[] boomHoles;
    public GameObject[] boomEffects;
    public GameObject[] zomDies;
    public GameObject[] blockDestroys;
    public GameObject[] zomHitOnRoads;
    public GameObject[] zomHitOnHeros;
    public GameObject playerDie;
    public GameObject towerExplosionHole;
    public GameObject towerExplosion;
    public Transform container;
    public int amoutHoleBullet;
    public int amoutZomeDie;
    public int amoutFlameThrower;
    public int amoutBoom;
    public int amoutBlockDestroy;
    public int amoutGunHitOnEnemy;
    public int amoutStunOnEnemy;
    public int amoutGunHitOnRoad;
    public int amoutZomHitOnRoad;
    public int amoutZomHitOnHero;
    int currentCountZomHitOnRoad;
    int currentCountZomHitOnHero;
    int currentCount;
    int currentCountFlameThrower;
    int currentCountZomDie;
    int currentCountBlockDestroy;
    int currentCountBoom;
    int currentCountGunHitOnRoad;
    int currentCountGunHitOnEnemy;
    int currentCountStunOnEnemy;

    private void Awake()
    {
        instance = this;
        Generate();
    }

    public void SetActivePar(bool isActive)
    {
        for (int i = 0; i < gunHitOnRoads.Length; i++)
        {
            if (gunHitOnRoads[i].activeSelf) gunHitOnRoads[i].SetActive(isActive);
        }
        for (int i = 0; i < flameThrowers.Length; i++)
        {
            if (flameThrowers[i].activeSelf) flameThrowers[i].SetActive(isActive);
        }
        for (int i = 0; i < stunOnEnemies.Length; i++)
        {
            if (stunOnEnemies[i].activeSelf) stunOnEnemies[i].SetActive(isActive);
        }
        for (int i = 0; i < gunHitOnEnemies.Length; i++)
        {
            if (gunHitOnEnemies[i].activeSelf) gunHitOnEnemies[i].SetActive(isActive);
        }
        for (int i = 0; i < gunHitExplosions.Length; i++)
        {
            if (gunHitExplosions[i].activeSelf) gunHitExplosions[i].SetActive(isActive);
        }
        for (int i = 0; i < roadBulletHole.Length; i++)
        {
            if (roadBulletHole[i].activeSelf) roadBulletHole[i].SetActive(isActive);
        }
        for (int i = 0; i < boomHoles.Length; i++)
        {
            if (boomHoles[i].activeSelf) boomHoles[i].SetActive(isActive);
        }
        for (int i = 0; i < boomEffects.Length; i++)
        {
            if (boomEffects[i].activeSelf) boomEffects[i].SetActive(isActive);
        }
        for (int i = 0; i < zomDies.Length; i++)
        {
            if (zomDies[i].activeSelf) zomDies[i].SetActive(isActive);
        }
        for (int i = 0; i < blockDestroys.Length; i++)
        {
            if (blockDestroys[i].activeSelf) blockDestroys[i].SetActive(isActive);
        }
        for (int i = 0; i < zomHitOnRoads.Length; i++)
        {
            if (zomHitOnRoads[i].activeSelf) zomHitOnRoads[i].SetActive(isActive);
        }
        for (int i = 0; i < zomHitOnHeros.Length; i++)
        {
            if (zomHitOnHeros[i].activeSelf) zomHitOnHeros[i].SetActive(isActive);
        }

        if (ParLv2.instance != null)
        {
            ParLv2.instance.SetActivePar(isActive);
        }
    }

    void Generate()
    {
        roadBulletHole = new GameObject[amoutHoleBullet];
        for (int i = 0; i < roadBulletHole.Length; i++)
        {
            roadBulletHole[i] = Instantiate(roadBulletHolePrefab, container);
            roadBulletHole[i].SetActive(false);
        }
        zomDies = new GameObject[amoutZomeDie];
        for (int i = 0; i < zomDies.Length; i++)
        {
            zomDies[i] = Instantiate(zomDiePrefab, container);
            zomDies[i].SetActive(false);
        }
        blockDestroys = new GameObject[amoutBlockDestroy];
        for (int i = 0; i < blockDestroys.Length; i++)
        {
            blockDestroys[i] = Instantiate(blockDestroyPrefab, container);
            blockDestroys[i].SetActive(false);
        }
        playerDie = Instantiate(playerDiePrefab, container);
        playerDie.SetActive(false);
        boomEffects = new GameObject[amoutBoom];
        for (int i = 0; i < boomEffects.Length; i++)
        {
            boomEffects[i] = Instantiate(boomEffectPrefab, container);
            boomEffects[i].SetActive(false);
        }
        boomHoles = new GameObject[amoutBoom];
        for (int i = 0; i < boomEffects.Length; i++)
        {
            boomHoles[i] = Instantiate(boomHolePrefab, container);
            boomHoles[i].SetActive(false);
        }
        towerExplosion = Instantiate(towerExplosionPrefab, container);
        towerExplosion.SetActive(false);
        towerExplosionHole = Instantiate(towerExplosionHolePrefab, container);
        towerExplosionHole.SetActive(false);
        gunHitExplosions = new GameObject[amoutGunHitOnRoad];
        gunHitOnRoads = new GameObject[amoutGunHitOnRoad];
        for (int i = 0; i < gunHitOnRoads.Length; i++)
        {
            gunHitExplosions[i] = Instantiate(gunHitExplosionPrefab, container);
            gunHitExplosions[i].SetActive(false);
            gunHitOnRoads[i] = Instantiate(gunHitOnRoadPrefab, container);
            gunHitOnRoads[i].SetActive(false);
        }
        gunHitOnEnemies = new GameObject[amoutGunHitOnEnemy];
        for (int i = 0; i < gunHitOnEnemies.Length; i++)
        {
            gunHitOnEnemies[i] = Instantiate(gunHitOnEnemyPrefab, container);
            gunHitOnEnemies[i].SetActive(false);
        }
        stunOnEnemies = new GameObject[amoutStunOnEnemy];
        for (int i = 0; i < stunOnEnemies.Length; i++)
        {
            stunOnEnemies[i] = Instantiate(stunOnEnemyPrefab, container);
            stunOnEnemies[i].SetActive(false);
        }
        flameThrowers = new GameObject[amoutFlameThrower];
        for (int i = 0; i < flameThrowers.Length; i++)
        {
            flameThrowers[i] = Instantiate(flameThrowerPrefab, container);
            flameThrowers[i].SetActive(false);
        }
        zomHitOnHeros = new GameObject[amoutZomHitOnHero];
        for (int i = 0; i < zomHitOnHeros.Length; i++)
        {
            zomHitOnHeros[i] = Instantiate(zomHitOnHeroPrefab, container);
            zomHitOnHeros[i].SetActive(false);
        }
        zomHitOnRoads = new GameObject[amoutZomHitOnRoad];
        for (int i = 0; i < zomHitOnRoads.Length; i++)
        {
            zomHitOnRoads[i] = Instantiate(zomHitOnRoadPrefab, container);
            zomHitOnRoads[i].SetActive(false);
        }
    }

    public void PlayZomHitOnRoadParticle(Vector2 pos)
    {
        GameObject h = zomHitOnRoads[currentCountZomHitOnRoad];
        h.transform.position = pos;
        h.SetActive(true);
        currentCountZomHitOnRoad++;
        if (currentCountZomHitOnRoad == zomHitOnRoads.Length) currentCountZomHitOnRoad = 0;
        DOVirtual.DelayedCall(1f, delegate { h.SetActive(false); });
    }

    public void PlayDestroyBlockParticle(Vector2 pos)
    {
        GameObject b = blockDestroys[currentCountBlockDestroy];
        b.transform.position = pos;
        b.SetActive(true);
        currentCountBlockDestroy++;
        if (currentCountBlockDestroy == blockDestroys.Length) currentCountBlockDestroy = 0;
        DOVirtual.DelayedCall(1f, delegate { b.SetActive(false); });
    }

    public void PlayZomHitOnHeroParticle(Vector2 pos)
    {
        GameObject h = zomHitOnHeros[currentCountZomHitOnHero];
        h.transform.position = pos;
        h.SetActive(true);
        currentCountZomHitOnHero++;
        if (currentCountZomHitOnHero == zomHitOnHeros.Length) currentCountZomHitOnHero = 0;
        DOVirtual.DelayedCall(1f, delegate { h.SetActive(false); });
    }

    public void PlayFlameThrowerParticle(Vector2 pos, Transform e, ref GameObject f)
    {
        f = flameThrowers[currentCountFlameThrower];
        f.transform.position = pos;
        f.transform.SetParent(e);
        f.SetActive(true);
        currentCountFlameThrower++;
        if (currentCountFlameThrower == flameThrowers.Length) currentCountFlameThrower = 0;
    }

    public void PlayStunOnEnemyParticle(Vector2 pos, Transform e, ref GameObject d)
    {
        d = stunOnEnemies[currentCountStunOnEnemy];
        d.transform.position = pos;
        d.SetActive(true);
        d.transform.SetParent(e);
        currentCountStunOnEnemy++;
        if (currentCountStunOnEnemy == stunOnEnemies.Length) currentCountStunOnEnemy = 0;
    }

    public void PlayRoadBulletHoleParticle(Vector2 pos)
    {
        GameObject d = roadBulletHole[currentCount];
        d.transform.position = pos;
        d.SetActive(true);
        currentCount++;
        if (currentCount == roadBulletHole.Length) currentCount = 0;
        DOVirtual.DelayedCall(3f, delegate { d.SetActive(false); });
    }

    public void PlayPlayerDieParticle(Vector2 pos)
    {
        playerDie.transform.position = pos;
        playerDie.SetActive(true);
        DOVirtual.DelayedCall(1f, delegate { playerDie.SetActive(false); });
    }

    public void PlayTowerExplosionParticle(Vector2 pos)
    {
        towerExplosion.transform.position = pos;
        towerExplosion.SetActive(true);
        towerExplosionHole.transform.position = pos;
        towerExplosionHole.SetActive(true);
        DOVirtual.DelayedCall(3f, delegate { towerExplosion.SetActive(false); towerExplosionHole.SetActive(false); });
    }

    public void PlayZomDieParticle(Vector2 pos)
    {
        GameObject d = zomDies[currentCountZomDie];
        d.transform.position = pos;
        d.SetActive(true);
        currentCountZomDie++;
        if (currentCountZomDie == zomDies.Length) currentCountZomDie = 0;
        DOVirtual.DelayedCall(1f, delegate { d.SetActive(false); });
    }

    public void PlayBlockDestroyParticle(Vector2 pos)
    {
        GameObject d = blockDestroys[currentCountBlockDestroy];
        d.transform.position = pos;
        d.SetActive(true);
        currentCountBlockDestroy++;
        if (currentCountBlockDestroy == blockDestroys.Length) currentCountBlockDestroy = 0;
        DOVirtual.DelayedCall(1f, delegate { d.SetActive(false); });
    }
    public void PlayBoomParticle(Vector2 pos)
    {
        GameObject b1 = boomEffects[currentCountBoom];
        GameObject b2 = boomHoles[currentCountBoom];
        b1.transform.position = pos;
        b2.transform.position = pos;
        b1.SetActive(true);
        b2.SetActive(true);
        currentCountBoom++;
        if (currentCountBoom == boomEffects.Length) currentCountBoom = 0;
        DOVirtual.DelayedCall(3f, delegate { b1.SetActive(false); b2.SetActive(false); });
    }

    public void PlayGunHitOnEnemyParticle(Vector2 pos)
    {
        GameObject g = gunHitOnEnemies[currentCountGunHitOnEnemy];
        g.transform.position = pos;
        g.SetActive(true);
        currentCountGunHitOnEnemy++;
        if (currentCountGunHitOnEnemy == gunHitOnEnemies.Length) currentCountGunHitOnEnemy = 0;
        DOVirtual.DelayedCall(3f, delegate { g.SetActive(false); });
    }

    public void PlayGunHitOnRoadParticle(Vector2 pos)
    {
        GameObject g1 = gunHitOnRoads[currentCountGunHitOnRoad];
        GameObject g2 = gunHitExplosions[currentCountGunHitOnRoad];
        g1.transform.position = pos;
        g2.transform.position = pos;
        g1.SetActive(true);
        g2.SetActive(true);
        currentCountGunHitOnRoad++;
        if (currentCountGunHitOnRoad == gunHitOnRoads.Length) currentCountGunHitOnRoad = 0;
        DOVirtual.DelayedCall(3f, delegate { g1.SetActive(false); g2.SetActive(false); });
    }
}
