using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/*
* Ovladac pre dealovanie damageu hracovi ked spadne do lavy.
*/
public class LavaController : MonoBehaviour
{
    GameObject player;
    HealthSystem health;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = player.GetComponent<HealthSystem>();
    }

    /*
    * Kym je hrac v lave, tak sa dealuje damage a je zobrazeny na HUDe.
    */
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            health.damage(1);
            health.ActivateDamageHUD();
        }
    }

    /*
    * Ak hrac vyskoci z lavy, tak damage na HUDe zmizne.
    */
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            health.DeactivateDamageHUD();
        }
    }
}
