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
    int minSwordDamage = 90;
    int maxSwordDamage = 110;

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
            swordDamage = Random.Range(minSwordDamage, maxSwordDamage);
            bossHealth.damage(swordDamage);
        }
    }

}
