using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public Transform player;
    Animator animator;
    NavMeshAgent npc;
    Transform playerTransform;
    [Header("General")]
    public int stage = 1;
    [Header("Stage 1")]
    [SerializeField] private bool beamPhase = false;
    [SerializeField] private bool goingToMiddle = false;
    [SerializeField] private bool inMiddle = false;
    [SerializeField] private bool pickupPolePhase = false;
    public Transform middlePoint;
    public float beamCooldownDefault = 10f;
    [SerializeField] private float beamCooldown;
    public float beamCastTime = 5f;
    public float beamDuration = 5f;
    [SerializeField] private float beamCast = -1;
    private Vector3 beamTargetPosition = Vector3.zero;
    public GameObject beamParticles;
    public GameObject bossPole;

    private GameObject[] poles;
    private GameObject lastPole = null;
    [Header("Stage 1 base attack")]
    public float attackCooldownDefault = 7f;



    [Header("Stage 2")]
    public GameObject groundHitParticles;
    public float groundHitCooldownDefault = 10f;
    [SerializeField] private float groundHitCooldown;
    public float groundHitEffectTime = .2f;
    public float groundHitCastTime = 5f;
    public float groundHitCast = -1f;
    public float groundHitDuration = 2f;
    [SerializeField] private bool groundHitPhase = false;
    public int groundHitCounter = 0;

    [Header("Stage 2 base attack")]
    public float bigAttackCooldownDefault = 7f;

    [Header("Base attack")]
    [SerializeField] private bool attackReady = false;
    [SerializeField] private float attackCooldown = 0;




    void Start()
    {
        animator = GetComponent<Animator>();
        npc = GetComponent<NavMeshAgent>();
        playerTransform = GetComponent<Transform>();
        beamCooldown = beamCooldownDefault;
        attackCooldown = bigAttackCooldownDefault;
        groundHitCooldown = groundHitCooldownDefault;
        //beamParticles = GameObject.FindWithTag("Beam");
        poles = GameObject.FindGameObjectsWithTag("Pole");
    }

    void stage1()
    {
        if (pickupPolePhase)
        {
            npc.stoppingDistance = 0f;
            if (npc.remainingDistance < 5f) // todo fix
            {
                // todo dust particles
                //GameObject.FindGameObjectWithTag("BossPole")
                bossPole.SetActive(true);
                lastPole.SetActive(false);
                stage = 2;
            }
            else
            {
                npc.SetDestination(lastPole.transform.position);
            }
        }else if (beamPhase){
            if (inMiddle)
            {
                npc.transform.LookAt(player.transform);
                if (beamTargetPosition == Vector3.zero)
                {
                    // cast beam
                    if (beamCast == -1)
                    {
                        beamCast = 0;
                    }
                    else
                    {
                        beamCast += Time.deltaTime;
                        if (beamCast >= beamCastTime)
                        {
                            beamTargetPosition = new Vector3(player.transform.position.x, 1.9f, player.transform.position.z);
                        }
                    }
                }
                else
                {
                    // beam casting animation
                    //Debug.DrawLine(new Vector3(transform.position.x, 5, transform.position.z), beamTargetPosition, Color.red);
                    beamParticles.SetActive(true);
                    beamCast += Time.deltaTime;
                    if (beamCast - beamCastTime >= beamDuration)
                    {
                        /*
                        if (Physics.Linecast(transform.position, player.position, out RaycastHit hitInfo))
                        {
                            Debug.Log("blocked");
                            Debug.Log(hitInfo);
                            hitInfo.collider.gameObject.SetActive(false);
                        }
                        */
                        int max = -1;
                        GameObject poleWithMax = null;
                        foreach (GameObject pole in poles.Where(p => p.activeSelf))
                        {
                            Debug.Log(pole.GetComponent<PoleHit>());
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
                        beamParticles.SetActive(false);
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
                if (goingToMiddle)
                {
                    npc.stoppingDistance = 0;
                    if (npc.remainingDistance <= 2f) // todo fix
                    {
                        //npc.enabled = false;
                        npc.transform.position = new Vector3(middlePoint.position.x, npc.transform.position.y, middlePoint.position.z);
                        inMiddle = true;
                        animator.SetBool("isFollowing", false);
                        //Debug.Log("inMiddle = true");
                    }
                    Debug.Log(npc.remainingDistance);
                }
                else
                {
                    //npc.enabled = true;
                    npc.SetDestination(middlePoint.position);
                    //Debug.Log("goingToMiddle = true");
                    goingToMiddle = true;
                }
            }
        }
        else
        {
            npc.SetDestination(player.position);
            npc.stoppingDistance = 7f;
            if (beamCooldown <= 0)
            {
                if (poles.Where(p => p.activeSelf).ToList().Count == 1)
                {
                    pickupPolePhase = true;
                    lastPole = poles.Where(p => p.activeSelf).ToList().First();
                }
                else
                {
                    beamPhase = true;
                    //animator.SetBool("isFollowing", true);
                    Debug.Log("Going to middle");
                }
            }
            else
            {
                beamCooldown -= Time.deltaTime;
                if (attackReady)
                {
                    if (npc.remainingDistance <= 8f)
                    {
                        animator.SetTrigger("attack");
                        attackReady = false;
                        attackCooldown = attackCooldownDefault;
                    }
                }
                else
                {
                    if (attackCooldown <= 0)
                    {
                        attackReady = true;
                    }
                    else
                    {
                        attackCooldown -= Time.deltaTime;
                    }
                }
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
        beamPhase = false;
        goingToMiddle = false;
        inMiddle = false;
        if (!bossPole.activeSelf)
        {
            bossPole.SetActive(true);
        }
        npc.SetDestination(player.position);
        npc.isStopped = false;

        if (groundHitPhase)
        {
            npc.stoppingDistance = 9999f;
            groundHitCast += Time.deltaTime;
            if (groundHitCast > groundHitCastTime)
            {
                if (groundHitCast - groundHitCastTime >= groundHitDuration)
                {
                    groundHitParticles.SetActive(true);
                    if (groundHitCast - groundHitCastTime - groundHitEffectTime >= groundHitDuration)
                    {
                        Debug.Log("ground hit damage");
                        Debug.Log("ground hit stop");
                        groundHitCooldown = groundHitCooldownDefault;
                        groundHitPhase = false;
                        groundHitParticles.SetActive(false);
                        if(Vector3.Distance(player.position, transform.position) < 20f)
                        {
                            Debug.Log("Player hit");
                        }
                        groundHitCounter++;
                        if (groundHitCounter > 4) {
                            stage = 3;
                            // TODO change scene to cave
                        }
                    }
                }
                else
                {
                    Debug.Log("ground hit animation");
                    groundHitParticles.SetActive(true);
                }
            }
        }
        else
        {
            npc.stoppingDistance = 7f;
            if (groundHitCooldown <= 0)
            {
                groundHitPhase = true;
                groundHitCast = 0f;
            }
            else
            {
                groundHitCooldown -= Time.deltaTime;
                if (attackReady)
                {
                    if (npc.remainingDistance <= 8f)
                    {
                        animator.SetTrigger("attack");
                        attackReady = false;
                        attackCooldown = bigAttackCooldownDefault;
                    }
                }
                else
                {
                    if (attackCooldown <= 0)
                    {
                        attackReady = true;
                    }
                    else
                    {
                        attackCooldown -= Time.deltaTime;
                    }
                }
            }
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

    void stage3()
    {
        npc.stoppingDistance = 7f;
        npc.SetDestination(player.position);
        npc.isStopped = false;
        List<GameObject> groundParts = GameObject.FindGameObjectsWithTag("GroundPart").ToList();

        foreach (var groundPart in groundParts)
        {
            if (Vector3.Distance(transform.position, groundPart.transform.position) < 5f)
            {
                Destroy(groundPart);
            }
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

    void Update()
    {
        switch (stage)
        {
            case 1:
                stage1();
                break;
            case 2:
                stage2();
                break;
            case 3:
                stage3();
                break;
        }
    }
}
