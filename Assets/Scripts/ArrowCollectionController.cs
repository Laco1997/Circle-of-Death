using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollectionController : MonoBehaviour
{
    private GameObject player;
    ArrowSystem arrowSys;

    private GameObject arrowSpawner;
    ArrowSpawner arrows;

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
            Debug.Log("Arrows picked up");
            arrowSys.arrowsCollected(10);

            arrows.arrowsPicked();
            arrows.resetTime();

            Destroy(gameObject);
        }
    }
}
