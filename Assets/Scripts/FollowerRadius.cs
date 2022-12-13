using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Ovladac pre spustenie boja, ked sa hrac priblizi k bossovi.
*/
public class FollowerRadius : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    Follower bossFollower;

    /*
     * Ziskanie skriptu Follower.
     */
    void Start()
    {
        bossFollower = boss.GetComponent<Follower>();
    }

    /*
    * Ak sa zaznamena kolizia medzi hracom a neviditelnym polom okolo bossa,
    * tak sa prehra hudba a boss zacne hraca prenasledovat.
    */
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && bossFollower.stage < 3)
        {
            bossFollower.checkPlayerEnter(true);
            FindObjectOfType<AudioManager>().Stop("PreFightMusic");
            FindObjectOfType<AudioManager>().Play("Stage1and2Music");
        }
    }
}
