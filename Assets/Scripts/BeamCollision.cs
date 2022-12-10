using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class BeamCollision : MonoBehaviour
{
    private HealthSystem hs;
    public int beamDamage = 10;
    void Start()
    {
        hs = GetComponent<HealthSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        hs.damage(beamDamage);
    }
}
