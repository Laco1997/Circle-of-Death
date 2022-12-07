using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
    }
}
