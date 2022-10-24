using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public Transform player;
    Animator animator;
    NavMeshAgent npc;
    Transform transform;
    private float attackCooldown = 0;
    private float attackTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        npc = GetComponent<NavMeshAgent>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        //Debug.Log(this.attackCooldown);
        if(distance < 5.5f)
            {
                if (this.attackCooldown <= 0)
                {
                    npc.enabled = false;
                    animator.SetBool("isFollowing", false);
                    animator.SetTrigger("attack");
                    this.attackCooldown = 7;
                    this.attackTimer = 0;
                }
            }
        else
            {
            if (npc.enabled)
            {
                npc.SetDestination(player.position);
            }

            if (npc.velocity.magnitude > 0.95)
            {
                animator.SetBool("isFollowing", true);
            }
            else
            {
                animator.SetBool("isFollowing", false);
            }
        }


         this.attackCooldown -= Time.deltaTime;
         this.attackTimer += Time.deltaTime;
        if(this.attackTimer > 3f)
        {
            npc.enabled = true;
        }
    }
}
