using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
* Ovladac pre zbieranie zivota a energie.
*/
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

    /*
    * Kolizia hraca s potion objektom.
    */
    void OnTriggerEnter(Collider col)
    {
        /*
        * Ak hrac prejde cez health potion, tak sa mu prida maximalny zivot,
        * resetne sa casovac pre spawnovanie healthu a objekt zmizne.
        */
        if (col.gameObject.tag == "Player" && gameObject.name == "Bottle_Health(Clone)")
        {
            health.healthGained(500);

            potions.healthPicked();
            potions.resetTime();

            Destroy(gameObject);
        }

        /*
        * Ak hrac prejde cez energy potion, tak sa mu prida maximalna energia,
        * resetne sa casovac pre spawnovanie energy a objekt zmizne.
        */
        if (col.gameObject.tag == "Player" && gameObject.name == "Bottle_Mana(Clone)")
        {
            energy.energyGained(500);

            potions.energyPicked();
            potions.resetTime();

            Destroy(gameObject);
        }
    }
}
