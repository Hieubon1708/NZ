using System.Collections;
using UnityEngine;

public class FlameHandler : MonoBehaviour
{
    public ParticleSystem flameOnceParticle;

    public void FlameOnceParticle()
    {
        flameOnceParticle.Play();
    }
}
