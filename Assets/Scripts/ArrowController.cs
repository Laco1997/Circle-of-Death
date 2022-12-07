using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;

    int arrowDamage = 5;

    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && gameObject.name == "Shot Arrow")
        {
            bossHealth.damage(arrowDamage);

            Destroy(gameObject);
        }
    }
}
