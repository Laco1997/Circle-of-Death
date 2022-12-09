using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;
    int swordDamage;
    int minSwordDamage = 90;
    int maxSwordDamage = 110;

    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && player.GetComponent<PlayerMovement>().isAttacking)
        {
            swordDamage = Random.Range(minSwordDamage, maxSwordDamage);
            bossHealth.damage(swordDamage);
        }
    }

}
