using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerRadius : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    Follower bossFollower;

    void Start()
    {
        bossFollower = boss.GetComponent<Follower>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            bossFollower.checkPlayerEnter(true);
            FindObjectOfType<AudioManager>().Stop("PreFightMusic");
            FindObjectOfType<AudioManager>().Play("Stage1and2Music");
        }
    }
}
