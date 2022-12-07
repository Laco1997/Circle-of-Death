using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrows;
    int spawnTime = 12;
    float time = 0f;
    bool arrowsPickedUp = true;

    int minX = 160;
    int maxX = 240;
    float y = 3f;
    int minZ = 150;
    int maxZ = 230;

    public void resetTime()
    {
        time = 0f;
    }

    public void arrowsPicked()
    {
        arrowsPickedUp = true;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        if (time >= spawnTime)
        {
            if (arrowsPickedUp)
            {
                SpawnArrows();
            }
        }
    }

    void SpawnArrows()
    {
        arrowsPickedUp = false;
        Vector3 arrowsRandPos = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
        Instantiate(arrows, arrowsRandPos, Quaternion.identity);
    }

}
