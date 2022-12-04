using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrows;
    int spawnTime = 12;
    float time = 0f;
    bool arrowsPickedUp = true;

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

        //Debug.Log(time);

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
        Vector3 arrowsRandPos = new Vector3(Random.Range(160, 240), 3f, Random.Range(150, 230));
        Instantiate(arrows, arrowsRandPos, Quaternion.identity);
    }

}
