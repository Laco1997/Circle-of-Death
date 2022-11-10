using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
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
    public float beamDuration = 5f;
    private float beamCast = -1;
    private Vector3 beamTargetPosition = Vector3.zero;
    public GameObject beamParticles;
    public GameObject boss_pole;

    public GameObject[] poles;
    private bool pickupPolePhase = false;
    private GameObject lastPole = null;



    public int stage = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        npc = GetComponent<NavMeshAgent>();
        playerTransform = GetComponent<Transform>();
        this.beamCooldown = this.beamCooldownDefault;
        //this.beamParticles = GameObject.FindWithTag("Beam");
        poles = GameObject.FindGameObjectsWithTag("Pole");
    }

    void stage1()
    {
        if (this.pickupPolePhase)
        {

            if (npc.remainingDistance < 5f) // todo fix
            {
                // todo dust particles
                //GameObject.FindGameObjectWithTag("BossPole")
                    boss_pole.SetActive(true);
                this.lastPole.SetActive(false);
                this.stage = 2;
            }
            else
            {
                npc.SetDestination(this.lastPole.transform.position);
            }
        }else if (this.beamPhase){
            if (this.inMiddle)
            {
                npc.transform.LookAt(player.transform);
                if (this.beamTargetPosition == Vector3.zero)
                {
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
                    //Debug.DrawLine(new Vector3(transform.position.x, 5, transform.position.z), this.beamTargetPosition, Color.red);
                    this.beamParticles.SetActive(true);
                    this.beamCast += Time.deltaTime;
                    if (this.beamCast - this.beamCastTime >= this.beamDuration)
                    {
                        /*
                        if (Physics.Linecast(this.transform.position, player.position, out RaycastHit hitInfo))
                        {
                            Debug.Log("blocked");
                            Debug.Log(hitInfo);
                            hitInfo.collider.gameObject.SetActive(false);
                        }
                        */
                        int max = -1;
                        GameObject poleWithMax = null;
                        foreach (GameObject pole in this.poles.Where(p => p.activeSelf))
                        {
                            int hits = pole.GetComponent<PoleHit>().hits;
                            if (hits > 0 && hits > max) // todo if hits > const
                            {
                                max = hits;
                                poleWithMax = pole;
                            }
                        }
                        if (poleWithMax)
                        {
                            poleWithMax.SetActive(false);
                        }
                        this.beamParticles.SetActive(false);
                            // stop cast
                        // destroy pole if targetted

                        //continue normally
                        beamPhase = false;
                        goingToMiddle = false;
                        inMiddle = false;
                        beamCooldown = beamCooldownDefault;
                        beamCast = 0;
                        beamTargetPosition = Vector3.zero;
}
                }
            }
            else
            {
                if (this.goingToMiddle)
                {
                    if(npc.remainingDistance < 2f) // todo fix
                    {
                        npc.enabled = false;
                        npc.transform.position = new Vector3(middlePoint.position.x, npc.transform.position.y, middlePoint.position.z);
                        this.inMiddle = true;
                        animator.SetBool("isFollowing", false);
                        //Debug.Log("inMiddle = true");
                    }
                    //Debug.Log(npc.remainingDistance);
                }
                else
                {
                    npc.enabled = true;
                    npc.SetDestination(middlePoint.position);
                    //Debug.Log("goingToMiddle = true");
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
                    if (this.poles.Where(p => p.activeSelf).ToList().Count == 1)
                    {
                        this.pickupPolePhase = true;
                    }
                    else
                    {
                        this.beamPhase = true;
                        animator.SetBool("isFollowing", true);
                        //Debug.Log("Going to middle");
                    }
                }
            }
            else
            {
                if (this.beamCooldown <= 0)
                {
                    if (this.poles.Where(p => p.activeSelf).ToList().Count == 1)
                    {
                        this.pickupPolePhase = true;
                        this.lastPole = this.poles.Where(p => p.activeSelf).First();
                    }
                    else
                    {
                        this.beamPhase = true;
                        animator.SetBool("isFollowing", true);
                        //Debug.Log("Going to middle");
                    }
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

    void stage2()
    {
        npc.SetDestination(player.position);
        Debug.Log("Stage2");
    }

    void Update()
    {
        switch (this.stage)
        {
            case 1:
                this.stage1();
                break;
            case 2:
                this.stage2();
                break;
        }
    }
}
