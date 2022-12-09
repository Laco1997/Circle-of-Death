using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;

    int arrowDamage;
    int minArrowDamage = 20;
    int maxArrowDamage = 25;

    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && gameObject.name == "Shot Arrow")
        {
            arrowDamage = Random.Range(minArrowDamage, maxArrowDamage);
            bossHealth.damage(arrowDamage);

            Destroy(gameObject);
        }
    }
}
