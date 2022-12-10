using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBossHit : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    GameObject player;
    HealthSystem health;
    public float attackHitDamageCooldownDefault = 5f;
    public int attackHitDamage = 200;
    public float attackHitDamageCooldown = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<HealthSystem>();
        attackHitDamageCooldown = 0;
    }
    void Update()
    {
        if (attackHitDamageCooldown > 0)
        {
            attackHitDamageCooldown -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (attackHitDamageCooldown <= 0 && boss.GetComponent<Follower>().bossIsAttacking)
            {
                health.damage(attackHitDamage);
                attackHitDamageCooldown = attackHitDamageCooldownDefault;
            }
        }
    }
}
