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
    public float attackHitDamageCooldownDefault = 3f;
    public int attackHitDamage = 200;
    public float attackHitDamageCooldown = 0;
    int minAttackHitDamage = 80;
    int maxAttackHitDamage = 120;

    /*
     * Ziskanie objektu hrac, skriptu HealthSystem a inicializacia cooldownu pre
     * attack na hodnotu 0.
     */
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
                attackHitDamage = UnityEngine.Random.Range(minAttackHitDamage, maxAttackHitDamage);

                if (boss.GetComponent<Follower>().stage == 2)
                {
                    health.damage(attackHitDamage * 2);
                }
                else
                {
                    health.damage(attackHitDamage);
                }
                attackHitDamageCooldown = attackHitDamageCooldownDefault;
            }
        }
    }
}
