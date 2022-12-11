using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Halvny ovladac pre spawnovanie sipov. Ovladac kontroluje zber sipov, 
 * casovac spawnu sipov, a vytvaranie instancii sipov.
 */
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

    /*
     * Reset casovaca spawnu sipov po tom, ako hrac pozbieral sipy.
     */
    public void resetTime()
    {
        time = 0f;
    }

    /*
     * Volanie metody pri kolizii so sipmi, ze boli hracom pozbierane.
     */
    public void arrowsPicked()
    {
        arrowsPickedUp = true;
    }

    /*
     * Casovac pre spawnovanie sipov.
     */
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

    /*
     * Spawnovanie sipov pomocou vytvarania instancii sipov.
     */
    void SpawnArrows()
    {
        arrowsPickedUp = false;
        Vector3 arrowsRandPos = new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ));
        Instantiate(arrows, arrowsRandPos, Quaternion.identity);
    }

}
