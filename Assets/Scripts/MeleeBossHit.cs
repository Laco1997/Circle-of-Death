using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBossHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
    }
}
