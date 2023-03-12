using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/*
* Hlavny ovladac spravu AI bossa.
*/
public class Follower : MonoBehaviour
{
    [Header("Boss skin")]
    public GameObject npcMesh;
    public Texture npcSkin;
    bool npcSkinChanged = false;
    float stoppingDistance = 11f;

    [Header("General")]
    public Transform player;
    Animator animator;
    NavMeshAgent npc;
    Transform playerTransform;
    bool playerInDistance = false;
    bool stage3MusicPlaying = false;
    public int stage = 1;
    GameObject playerObject;
    HealthSystem health;
    public bool bossIsAttacking;
    [SerializeField] GameObject swordHitRadius;
    [SerializeField] GameObject protectingShield;
    [SerializeField] GameObject wallAround;
    int minGroundDamage = 250;
    int maxGroundDamage = 350;

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
    Vector3 beamTargetPosition = Vector3.zero;
    public GameObject beamParticles;
    public GameObject bossPole;
    GameObject[] poles;
    GameObject lastPole = null;

    [Header("Stage 1 base attack")]
    public float attackCooldownDefault = 7f;
    public float attackDuration = 1.5f;

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
    public float groundHitRange = 20f;

    [Header("Stage 2 base attack")]
    public float bigAttackCooldownDefault = 7f;
    public float bigAttackDuration = 1.5f;

    [Header("Stage 3")]
    public float groundBreakCooldownDefault = 10f;
    [SerializeField] private float groundBreakCooldown;
    public float groundBreakEffectTime = .2f;
    public float groundBreakCastTime = 5f;
    public float groundBreakCast = -1f;
    public float groundBreakDuration = 2f;
    [SerializeField] private bool groundBreakPhase = false;
    public float groundBreakRange = 20f;

    [Header("Stage 3 base attack")]
    public float baseAttackCooldownP3Default = 7f;

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
        poles = GameObject.FindGameObjectsWithTag("Pole");
        playerObject = GameObject.FindGameObjectWithTag("Player");
        health = playerObject.GetComponent<HealthSystem>();
        bossIsAttacking = false;

        if(stage == 3)
        {
            playerInDistance = true;
        }
    }

    /*
    * Stage 1 hry, kedy boss followuje a vykonava swing attack voci hracovi.
    */
    void stage1()
    {
        // ak idem pre posledny stlp
        if (pickupPolePhase)
        {
            npc.stoppingDistance = 0f;
            // ak som pri nom (5  kvoli tomu  ze ak bolo 0 nikdy neprisiel)
            if (npc.remainingDistance < 5f)
            {
                bossPole.SetActive(true);
                lastPole.SetActive(false);
                stage = 2;
            }
            else
            {
                npc.SetDestination(lastPole.transform.position);
            }
        }
        else if (beamPhase)
        {
            if (inMiddle)
            {
                swordHitRadius.SetActive(false);
                npc.transform.LookAt(player.transform);
                // ak este necastim beam je cooldown
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
                            // docastil som beam a mierim na hraca (tento vektor sa pouzival ked boss nebol locknuty na hraca to sa zmenilo neskor)
                            beamTargetPosition = new Vector3(player.transform.position.x, 1.9f, player.transform.position.z);
                        }
                    }
                }
                else
                {
                    // beam casting animation
                    beamParticles.SetActive(true);
                    beamCast += Time.deltaTime;
                    // ak som docastil
                    if (beamCast - beamCastTime >= beamDuration)
                    {
                        /*
                         * najdem stlp s najvacsim poskodenim od beamu
                         */
                        int max = -1;
                        GameObject poleWithMax = null;
                        foreach (GameObject pole in poles.Where(p => p.activeSelf))
                        {
                            int hits = pole.GetComponent<PoleHit>().hits;
                            if (hits > 0 && hits > max)
                            {
                                max = hits;
                                poleWithMax = pole;
                            }
                        }
                        // odstranim stlp 
                        if (poleWithMax)
                        {
                            poleWithMax.SetActive(false);
                        }
                        beamParticles.SetActive(false);
                        // stop cast
                        // vyresetujem premenne na normalny stav
                        beamPhase = false;
                        goingToMiddle = false;
                        inMiddle = false;
                        swordHitRadius.SetActive(true);
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
                    // ak je vzdialenost skoro v cieli (ak by bola 0 tak boss nikdy nedosiel k cielu)
                    if (npc.remainingDistance <= 2f)
                    {
                        // preistotu nastavim poziciu na stred 
                        npc.transform.position = new Vector3(middlePoint.position.x, npc.transform.position.y, middlePoint.position.z);
                        inMiddle = true;
                        animator.SetBool("isFollowing", false);
                    }
                }
                else
                {
                    npc.SetDestination(middlePoint.position);
                    goingToMiddle = true;
                }
            }
        }
        else
        {
            npc.SetDestination(player.position);
            npc.stoppingDistance = stoppingDistance;
            if (beamCooldown <= 0)
            {
                // ak ostal posledny stlp idem ho zobrat
                if (poles.Where(p => p.activeSelf).ToList().Count == 1)
                {
                    pickupPolePhase = true;
                    lastPole = poles.Where(p => p.activeSelf).ToList().First();
                }
                else
                {
                    beamPhase = true;
                }
            }
            else
            {
                if (bossIsAttacking)
                {
                    npc.stoppingDistance = 9999f;
                    // stop boss when attacking
                    attackCooldown -= Time.deltaTime;
                    if (attackCooldown*-1 > attackDuration)
                    {
                        npc.stoppingDistance = stoppingDistance;
                        bossIsAttacking = false;
                        attackReady = false;
                        attackCooldown = attackCooldownDefault;
                    }
                }
                else
                {
                    beamCooldown -= Time.deltaTime;
                    // ak moze pouzit melee utok
                    if (attackReady)
                    {
                        // ak je boss v blizkosti hraca
                        if (npc.remainingDistance <= stoppingDistance)
                        {
                            bossIsAttacking = true;
                            animator.SetTrigger("attack");
                        }
                    }
                    else
                    {
                        // nacitanie utoku
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
            
            // ak boss ide rychlejsie zmenim animaciu na pohyb
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
            // pre testovanie stage 2 aby bol stlp v ruke vzdy aktivny v S2
            bossPole.SetActive(true);
        }
        npc.SetDestination(player.position);
        npc.isStopped = false;

        if (groundHitPhase)
        {
            // zastavim bossa na mieste
            npc.stoppingDistance = 9999f;
            groundHitCast += Time.deltaTime;
            if (groundHitCast > groundHitCastTime)
            {
                // ak sa dokoncil cast ground hitu
                if (groundHitCast - groundHitCastTime >= groundHitDuration)
                {
                    // ak sa dokoncil cast a aj animacia hitu
                    groundHitParticles.SetActive(true);
                    var sh = groundHitParticles.GetComponent<ParticleSystem>().shape;
                    sh.radius = groundHitRange;

                    if (groundHitCast - groundHitCastTime - groundHitEffectTime >= groundHitDuration)
                    {
                        // ak sa dokoncil cast aj animacia hitu aj particle effekt
                        groundHitCooldown = groundHitCooldownDefault;
                        groundHitPhase = false;
                        groundHitParticles.SetActive(false);
                        if(Vector3.Distance(player.position, transform.position) < groundHitRange)
                        {
                            // ak bol hrac v dosahu dostane damage
                            health.TimedHitDamage(1.5f);
                            int groundDamage = UnityEngine.Random.Range(minGroundDamage, maxGroundDamage);
                            health.damage(groundDamage);
                        }
                        groundHitCounter++;
                        if (groundHitCounter >= 2)
                        {
                            // po dvoch hitoch do zeme sa zem znici a prejde sa do novej sceny
                            groundHitPhase = false;
                            groundBreakCooldown = groundBreakCooldownDefault;
                            attackCooldown = baseAttackCooldownP3Default;
                            groundBreakPhase = false;
                            groundHitParticles.SetActive(false);
                            SceneManager.LoadScene("Cutscene");
                        }
                    }
                }
                else
                {
                    groundHitParticles.SetActive(true);
                }
            }
        }
        else
        {
            // rovnaka mechanika ako v stage 1 len iny spell
            npc.stoppingDistance = stoppingDistance;
            if (groundHitCooldown <= 0)
            {
                groundHitPhase = true;
                groundHitCast = 0f;
            }
            else
            {
                if (bossIsAttacking)
                {
                    npc.stoppingDistance = 9999f;
                    // stop boss when attacking
                    attackCooldown -= Time.deltaTime;
                    if (attackCooldown * -1 > bigAttackDuration)
                    {
                        npc.stoppingDistance = stoppingDistance;
                        bossIsAttacking = false;
                        attackReady = false;
                        attackCooldown = attackCooldownDefault;
                    }
                }
                else
                {
                    groundHitCooldown -= Time.deltaTime;
                    if (attackReady)
                    {
                        if (npc.remainingDistance <= 8f)
                        {
                            bossIsAttacking = true;
                            animator.SetTrigger("attack");
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
        npc.stoppingDistance = stoppingDistance;
        npc.SetDestination(player.position);
        npc.isStopped = false;
        // rovnaka logika ako v stage 2
        if (groundBreakPhase)
        {
            npc.stoppingDistance = 9999f;
            groundBreakCast += Time.deltaTime;
            if (groundBreakCast > groundBreakCastTime)
            {
                if (groundBreakCast - groundBreakCastTime >= groundBreakDuration)
                {
                    groundHitParticles.SetActive(true);
                    var sh = groundHitParticles.GetComponent<ParticleSystem>().shape;
                    sh.radius = groundBreakRange;
                    if (groundBreakCast - groundBreakCastTime - groundBreakEffectTime >= groundBreakDuration)
                    {
                        // najdem casti podlahy v dosahu a znicim ich
                        Collider[] hitColliders = Physics.OverlapSphere(transform.position, groundBreakRange, LayerMask.GetMask("GroundPart"));
                        foreach (var hitCollider in hitColliders)
                        {
                            Destroy(hitCollider.gameObject);
                        }
                        groundBreakCooldown = groundBreakCooldownDefault;
                        groundBreakPhase = false;
                        groundHitParticles.SetActive(false);
                        if (Vector3.Distance(player.position, transform.position) < groundBreakRange)
                        {
                            // ak je v dosahu aj hrac dostane damage
                            health.TimedHitDamage(1.5f);
                            int groundDamage = UnityEngine.Random.Range(minGroundDamage, maxGroundDamage);
                            health.damage(groundDamage);
                        }
                    }
                }
                else
                {
                    groundHitParticles.SetActive(true);
                }
            }
        }
        else
        {
            npc.stoppingDistance = stoppingDistance;
            if (groundBreakCooldown <= 0)
            {
                groundBreakPhase = true;
                groundBreakCast = 0f;
            }
            else
            {
                //groundBreakCooldown -= Time.deltaTime;
                if (bossIsAttacking)
                {
                    npc.stoppingDistance = 9999f;
                    // stop boss when attacking
                    attackCooldown -= Time.deltaTime;
                    if (attackCooldown * -1 > attackDuration)
                    {
                        npc.stoppingDistance = stoppingDistance;
                        bossIsAttacking = false;
                        attackReady = false;
                        attackCooldown = baseAttackCooldownP3Default;
                    }
                }
                else
                {
                    groundBreakCooldown -= Time.deltaTime;
                    if (attackReady)
                    {
                        if (npc.remainingDistance <= 8f)
                        {
                            bossIsAttacking = true;
                            animator.SetTrigger("attack");
                            attackReady = false;
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

    public void checkPlayerEnter(bool playerEntered)
    {
        playerInDistance = playerEntered;
        protectingShield.SetActive(false);
        wallAround.SetActive(true);
    }

    void Update()
    {
        health.mainWorldData();
        gameObject.GetComponent<HealthSystem>().mainWorldData();

        if (playerInDistance)
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
                    changeSkin();
                    stage3();
                    playStage3Music();
                    break;
            }
        }

        if (playerInDistance)
        {
            npc.isStopped = false;
        }
        else
        {
            npc.isStopped = true;
        }
    }

    /*
    * Zmena skinu bossa pri jednotlivych leveloch hry.
    */
    void changeSkin()
    {
        if(!npcSkinChanged)
        {
            npcSkinChanged = true;
            npcMesh.GetComponent<Renderer>().material.EnableKeyword("_DETAIL_MULX2");
            npcMesh.GetComponent<Renderer>().material.SetTexture("_DetailAlbedoMap", npcSkin);
            npcMesh.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
        }
    }

    /*
    * Zmena hudby pri vstupe do lava sveta.
    */
    void playStage3Music()
    {
        if (!stage3MusicPlaying)
        {
            stage3MusicPlaying = true;
            FindObjectOfType<AudioManager>().Stop("PreFightMusic");
            FindObjectOfType<AudioManager>().Stop("Stage1and2Music");
            FindObjectOfType<AudioManager>().Play("Stage3Music");
        }
    }
}
