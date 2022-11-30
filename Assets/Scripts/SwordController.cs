using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject boss;
    HealthSystem bossHealth;

    // Start is called before the first frame update
    void Start()
    {
        bossHealth = boss.GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" && player.GetComponent<PlayerMovement>().isAttacking)
        {
            Debug.Log("Sword hit");
            bossHealth.damage(5);
        }
    }

}
