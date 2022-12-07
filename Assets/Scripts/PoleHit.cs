using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleHit : MonoBehaviour
{
    public int hits = 0;

    void OnParticleCollision(GameObject other)
    {
        hits++;
    }
}
