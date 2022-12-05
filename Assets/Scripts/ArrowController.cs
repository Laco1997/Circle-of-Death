using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;

    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && gameObject.name == "Shot Arrow")
        {
            Debug.Log("Arrow hit");
            bossHealth.damage(5);

            Destroy(gameObject);
        }
    }
}
