using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre hitovanie hraca pomocou swingu. Boss ma cooldown po kazdom hite.
*/
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

    /*
    * Cooldown casovac po uskutocneni attacku bossom.
    */
    void Update()
    {
        if (attackHitDamageCooldown > 0)
        {
            attackHitDamageCooldown -= Time.deltaTime;
        }
    }

    /*
    * Ak boss hitol hraca, tak sa hracovi zobrazi damage na HUDe a odrata sa
    * mu zo zivota.
    */
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (attackHitDamageCooldown <= 0 && boss.GetComponent<Follower>().bossIsAttacking)
            {
                health.TimedHitDamage(1.5f);
                health.damage(attackHitDamage);
                attackHitDamageCooldown = attackHitDamageCooldownDefault;
            }
        }
    }
}
