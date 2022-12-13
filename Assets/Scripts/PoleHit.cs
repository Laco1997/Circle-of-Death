using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre hitovanie hraca so stlpom.
*/
public class PoleHit : MonoBehaviour
{
    public int hits = 0;

    /*
     * Pri kolizii particles beamu sa prirata hit hracovi.
     */
    void OnParticleCollision(GameObject other)
    {
        hits++;
    }
}
