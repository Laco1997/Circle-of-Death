using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PotionController : MonoBehaviour
{
    private GameObject player;
    EnergySystem energy;
    HealthSystem health;

    private GameObject potionSpawner;
    PotionSpawner potions;

    // Start is called before the first frame update
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
            Debug.Log("Health picked up");
            health.healthGained(500);

            potions.healthPicked();
            potions.resetTime();

            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Player" && gameObject.name == "Bottle_Mana(Clone)")
        {
            Debug.Log("Energy picked up");
            energy.energyGained(500);

            potions.energyPicked();
            potions.resetTime();

            Destroy(gameObject);
        }
    }
}
