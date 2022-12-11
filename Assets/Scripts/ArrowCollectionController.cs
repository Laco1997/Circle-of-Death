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
    int arrowsCount = 10;

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
        if (col.gameObject.tag == "Player" && gameObject.name == "ArrowCollection(Clone)")
        {
            arrowSys.arrowsCollected(arrowsCount);

            arrows.arrowsPicked();
            arrows.resetTime();

            Destroy(gameObject);
        }
    }
}
