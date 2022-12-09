using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    GameObject player;
    EnergySystem energy;
    HealthSystem health;

    GameObject potionSpawner;
    PotionSpawner potions;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        potionSpawner = GameObject.FindGameObjectWithTag("PotionSpawner");

        potions = potionSpawner.GetComponent<PotionSpawner>();
        energy = player.GetComponent<EnergySystem>();
        health = player.GetComponent<HealthSystem>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && gameObject.name == "Bottle_Health(Clone)")
        {
            health.healthGained(500);

            potions.healthPicked();
            potions.resetTime();

            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Player" && gameObject.name == "Bottle_Mana(Clone)")
        {
            energy.energyGained(500);

            potions.energyPicked();
            potions.resetTime();

            Destroy(gameObject);
        }
    }
}
