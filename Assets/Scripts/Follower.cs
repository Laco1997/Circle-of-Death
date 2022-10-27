using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public Transform player;
    Animator animator;
    NavMeshAgent npc;
    Transform playerTransform;
    private float attackCooldown = 0;
    private float attackTimer = 0;

    private bool beamPhase = false;
    private bool goingToMiddle = false;
    private bool inMiddle = false;
    public Transform middlePoint;
    public float beamCooldownDefault = 10f;
    private float beamCooldown;
    public float beamCastTime = 5f;
    private float beamCast = -1;
    private Vector3 beamTargetPosition = Vector3.zero;

    public int stage = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        npc = GetComponent<NavMeshAgent>();
        playerTransform = GetComponent<Transform>();
        this.beamCooldown = this.beamCooldownDefault;
    }

    void stage1()
    {
        if (this.beamPhase)
        {
            if (this.inMiddle)
            {
                if (this.beamTargetPosition == Vector3.zero)
                {
                    npc.transform.LookAt(player.transform);
                    // cast beam
                    if (this.beamCast == -1)
                    {
                        this.beamCast = 0;
                    }
                    else
                    {
                        this.beamCast += Time.deltaTime;
                        if (this.beamCast >= this.beamCastTime)
                        {
                            this.beamTargetPosition = new Vector3(player.transform.position.x, 1.9f, player.transform.position.z);
                            
                        }
                    }
                }
                else
                {
                    // beam casting animation
                    Debug.DrawLine(new Vector3(transform.position.x, 5, transform.position.z), this.beamTargetPosition, Color.red);
                }
            }
            else
            {
                if (this.goingToMiddle)
                {
                    if(npc.remainingDistance < 0.5)
                    {
                        npc.enabled = false;
                        npc.transform.position = new Vector3(middlePoint.position.x, npc.transform.position.y, middlePoint.position.z);
                        this.inMiddle = true;
                        animator.SetBool("isFollowing", false);
                        Debug.Log("inMiddle = true");
                    }
                    Debug.Log(npc.remainingDistance);
                }
                else
                {
                    npc.SetDestination(middlePoint.position);
                    Debug.Log("goingToMiddle = true");
                    this.goingToMiddle = true;
                }
            }
        }
        else
        {
            float distance = Vector3.Distance(player.position, transform.position);
            //Debug.Log(this.attackCooldown);
            if (distance < 5.5f)
            {
                if (this.attackCooldown <= 0)
                {
                    npc.enabled = false;
                    animator.SetBool("isFollowing", false);
                    animator.SetTrigger("attack");
                    this.attackCooldown = 7;
                    this.attackTimer = 0;
                }
                else if (this.beamCooldown <= 0)
                {
                    this.beamPhase = true;
                    animator.SetBool("isFollowing", true);
                    Debug.Log("Going to middle");
                }
            }
            else
            {
                if (this.beamCooldown <= 0)
                {
                    this.beamPhase = true;
                    animator.SetBool("isFollowing", true);
                    Debug.Log("Going to middle");
                }
                else
                {
                    if (npc.enabled)
                    {
                        npc.SetDestination(player.position);
                    }
                }
            }


            this.attackCooldown -= Time.deltaTime;
            this.beamCooldown -= Time.deltaTime;
            this.attackTimer += Time.deltaTime;
            if (this.attackTimer > 3f)
            {
                npc.enabled = true;
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

    }

    void Update()
    {
        switch (this.stage)
        {
            case 1:
                this.stage1();
                break;
        }
    }
}
