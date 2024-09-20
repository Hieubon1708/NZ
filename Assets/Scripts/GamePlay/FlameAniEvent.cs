using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAniEvent : MonoBehaviour
{
    public ParticleSystem flameOnceParticle;

    public void FlameOnceParticle()
    {
        flameOnceParticle.Play();
    }
}
