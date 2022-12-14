using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ovladac pre zbieranie sipov. Hrac zbiera 10 sipov pomocou kolizie medzi 
 * hracom a sipmi. Nasledne sa casovac pre spawn sipov resetuje.
 */
public class ArrowCollectionController : MonoBehaviour
{
    GameObject player;
    ArrowSystem arrowSys;
    GameObject arrowSpawner;
    ArrowSpawner arrows;
    int arrowsCount = 3;

    /*
     * Ziskanie objektov a skriptov.
     */
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        arrowSpawner = GameObject.FindGameObjectWithTag("ArrowSpawner");

        arrows = arrowSpawner.GetComponent<ArrowSpawner>();
        arrowSys = player.GetComponent<ArrowSystem>();
    }

    /*
     * Kolizia medzi sipmi a hracom, pre zbieranie sipov.
     */
    void OnTriggerEnter(Collider col)
    {
        /*
         * Ak je kolizia medzi hracom a kolekciou sipov, tak sa v skripte ArrowSystem
         * zavola funkcia arrowsCollected() a pridaju sa sipy. Nasledne sa potvrdi 
         * pozbieranie sipov a resetne sa cas. Sipy po zbere zmiznu.
         */
        if (col.gameObject.tag == "Player" && gameObject.name == "ArrowCollection(Clone)")
        {
            arrowSys.arrowsCollected(arrowsCount);

            arrows.arrowsPicked();
            arrows.resetTime();

            Destroy(gameObject);
        }
    }
}
