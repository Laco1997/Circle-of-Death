using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollectionController : MonoBehaviour
{
    private GameObject player;
    ArrowSystem arrowSys;

    private GameObject arrowSpawner;
    ArrowSpawner arrows;

    int arrowsCount = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        arrowSpawner = GameObject.FindGameObjectWithTag("ArrowSpawner");

        arrows = arrowSpawner.GetComponent<ArrowSpawner>();
        arrowSys = player.GetComponent<ArrowSystem>();
    }

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
