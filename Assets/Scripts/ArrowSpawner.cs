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
    float y = 3f;
    GameObject boss;
    Follower bossFollower;

    /*
     * Ziskanie skriptu Follower.
     */
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Enemy");
        bossFollower = boss.GetComponent<Follower>();
    }

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
        Vector3 arrowsRandPos = new Vector3(Random.Range(180, 240), y, Random.Range(165, 225));
        if (bossFollower.stage == 3)
        {
            arrowsRandPos = new Vector3(Random.Range(50, 160), y, Random.Range(50, 140));
        }
        Instantiate(arrows, arrowsRandPos, Quaternion.identity);
    }

}
