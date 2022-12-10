using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class LavaController : MonoBehaviour
{
    GameObject player;
    HealthSystem health;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<HealthSystem>();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            health.damage(1);
        }
    }
}
