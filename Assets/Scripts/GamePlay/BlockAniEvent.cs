using UnityEngine;

public class BlockAniEvent : MonoBehaviour
{
    public ParticleSystem upgradeBlock;

    public void PlayUpgradeParticle()
    {
        upgradeBlock.Play();
    }
}
