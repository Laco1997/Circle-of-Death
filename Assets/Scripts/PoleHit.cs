using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre hitovanie hraca so stlpom.
*/
public class PoleHit : MonoBehaviour
{
    public int hits = 0;

    void OnParticleCollision(GameObject other)
    {
        hits++;
    }
}
