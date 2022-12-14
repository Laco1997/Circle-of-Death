using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

/*
* Ovladac pre hitovanie bossa so swordom.
*/
public class SwordController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;
    int swordDamage;
    int minSwordDamage = 110;
    int maxSwordDamage = 180;
    float criticalChance = 0.3f;

    /*
     * Ziskanie skriptu HealthSystem.
     */
    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    /*
    * Ak bol boss hitnuty so swordom, tak sa mu odrata zivot.
    */
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "SwordRadius" && player.GetComponent<PlayerMovement>().isAttacking)
        {
            float normChance = Random.Range(0f, 1f);
            swordDamage = Random.Range(minSwordDamage, maxSwordDamage);

            if (normChance <= criticalChance)
            {
                swordDamage *= 2;
            }

            bossHealth.damage(swordDamage);
        }
    }

}
