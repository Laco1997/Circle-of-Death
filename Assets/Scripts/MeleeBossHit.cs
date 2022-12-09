using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBossHit : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    GameObject player;
    HealthSystem health;
    bool isTriggered;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<HealthSystem>();
        isTriggered = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !isTriggered && boss.GetComponent<Follower>().bossIsAttacking)
        {
            isTriggered = true;
            health.damage(200);
            boss.GetComponent<Follower>().bossIsAttacking = false;
        }
    }

    void OnTriggerExit(Collider col)
    {
        isTriggered = false;
    }
}
