using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ovladac pre hitovanie bossa pomocou sipu. Vyska damageu sipom je random v
 * intervale medzi 20-25. Po hite so sipom, sa zivot bossa znizi.
 */
public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;
    int arrowDamage;
    int minArrowDamage = 80;
    int maxArrowDamage = 125;
    float criticalChance = 0.4f;

    /*
    * Ziskanie skriptu HealthSystem.
    */
    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    /*
     * Kolizia medzi sipom a bossom, pre dealovanie damageu.
     */
    void OnTriggerEnter(Collider col)
    {
        /*
         * Ak je kolizia medzi bossom a vystrelenym sipom, tak sa v skripte HealthSystem
         * zavola funkcia damage() a odoberie sa HP bossovi. Vystreleny sip po kolizii
         * zmizne.
         */
        if (col.gameObject.tag == "Enemy" && gameObject.name == "Shot Arrow")
        {
            float normChance = Random.Range(0f, 1f);
            arrowDamage = Random.Range(minArrowDamage, maxArrowDamage);

            if (normChance <= criticalChance)
            {
                arrowDamage *= 2;
            }

            bossHealth.damage(arrowDamage);

            Destroy(gameObject);
        }
    }
}
