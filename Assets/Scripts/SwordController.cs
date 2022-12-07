using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;
    int swordDamage = 5;

    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && player.GetComponent<PlayerMovement>().isAttacking)
        {
            bossHealth.damage(swordDamage);
        }
    }

}
