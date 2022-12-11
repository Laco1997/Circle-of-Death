using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

/*
* Ovladac pre damageovanie hraca s beamom pomocou kolizie.
*/
public class BeamCollision : MonoBehaviour
{
    private HealthSystem hs;
    public int beamDamage = 10;

    void Start()
    {
        hs = GetComponent<HealthSystem>();
    }

    /*
    * Udelenie damageu hracovi a zobrazenie damage hit na HUD pri kolizii s beamom.
    */
    void OnParticleCollision(GameObject other)
    {
        hs.TimedHitDamage(3f);
        hs.damage(beamDamage);
    }
}
