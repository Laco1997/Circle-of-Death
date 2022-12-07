using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using static Cinemachine.CinemachineOrbitalTransposer;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject arms;

    private Animator animator;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject arrow;
    private bool equippedSword = false;
    private bool equippedBow = false;
    private bool canAttack = true;
    public bool isAttacking = false;
    [SerializeField] private float attackCooldownTime = 0.45f;

    [SerializeField] private CharacterController controller;
    private float horizontal;
    private float vertical;

    [Header("Walk")]
    [SerializeField] private float walkSpeed = 3.2f;
    private Vector3 move;
    bool walking = false;
    bool walkAudioPlaying = false;

    [Header("Jump")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;
    private bool onGround;

    [Header("Sprint")]
    [SerializeField] private float sprintSpeed = 3.5f;
    bool sprinting = false;
    bool sprintAudioPlaying = false;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 4.5f;
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashDuration = 7f;
    [SerializeField] private float dashCurrentTime = 0f;
    [SerializeField] private float dashCooldownTime = 1.5f;
    private bool canDash = true;

    [Header("Shoot")]
    public float shootForce;
    public float upwardForce;
    public Camera cam;
    public Transform attackPoint;
    private bool canShoot = true;
    [SerializeField] private float shootCooldownTime = 2f;
    bool holding = false;
    bool hasArrows = true;
    ArrowSystem arrowSys;
    private int currentArrowCount;

    [Header("Weapons")]
    [SerializeField] private GameObject swordIcon;
    [SerializeField] private GameObject bowIcon;
    [SerializeField] private GameObject handsIcon;

    [Header("Energy")]
    EnergySystem playerEnergy;
    private int currentEnergy;
    private bool canDashWithEnergy = true;
    private bool canSprintWithEnergy = true;

    private void Start()
    {
        animator = GetComponentInChildren(typeof(Animator)) as Animator;
        sword.SetActive(false);
        bow.SetActive(false);
        arrow.SetActive(false);

        handsIcon.SetActive(true);
        swordIcon.SetActive(false);
        bowIcon.SetActive(false);

        playerEnergy = gameObject.GetComponent<EnergySystem>();
        currentEnergy = playerEnergy.CurrentEnergy;

        arrowSys = gameObject.GetComponent<ArrowSystem>();
        currentArrowCount = arrowSys.CurrentArrows;
    }

    void Update()
    {
        // Ground check
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (onGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        CheckArrows();

        CheckEnergy();

        PlayerInput();

        MovePlayer();

        WalkAudio();

        SprintAudio();

    }

    private void PlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void WalkAudio()
    {
        if (walking && !walkAudioPlaying)
        {
            FindObjectOfType<AudioManager>().Play("Walk");
            walkAudioPlaying = true;
        }
        else if(!walking && walkAudioPlaying)
        {
            FindObjectOfType<AudioManager>().Stop("Walk");
            walkAudioPlaying = false;
        }
    }

    void SprintAudio()
    {
        if (sprinting && !sprintAudioPlaying)
        {
            FindObjectOfType<AudioManager>().Play("Sprint");
            FindObjectOfType<AudioManager>().PlayDelayed("SprintVoice", 1f);
            sprintAudioPlaying = true;
        }
        else if (!sprinting && sprintAudioPlaying)
        {
            FindObjectOfType<AudioManager>().Stop("Sprint");
            FindObjectOfType<AudioManager>().Stop("SprintVoice");
            FindObjectOfType<AudioManager>().Play("AfterSprintVoice");
            sprintAudioPlaying = false;
        }
    }

    private void MovePlayer()
    {
        move = transform.right * horizontal + transform.forward * vertical;

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            animator.SetBool("Walk", true);

            walking = true;

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                sprinting = false;
                animator.SetBool("Sprint", false);
            }
        }

        if ((Mathf.Abs(horizontal) == 0) && (Mathf.Abs(vertical) == 0))
        {
            walking = false;
            sprinting = false;

            animator.SetBool("Walk", false);
            animator.SetBool("Sprint", false);
        }

        controller.Move(move * walkSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && onGround)
        {
            if (animator.GetBool("Equipped Sword") == false && animator.GetBool("Equipped Bow") == false)
            {
                walking = false;
                sprinting = false;
                FindObjectOfType<AudioManager>().Play("Jump");

                animator.SetTrigger("Jump");
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -1 * gravity);
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift) && canSprintWithEnergy)
        {
            if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
            {
                walking = false;
                sprinting = true;
                //playerEnergy.energyUsed(1);
                animator.SetBool("Sprint", true);
            }

            controller.Move(move * sprintSpeed * Time.deltaTime);
        }

        // Weapon equip
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Unequip
            if (animator.GetBool("Equipped Sword") == true)
            {
                animator.SetTrigger("Unequip");
                animator.SetBool("Equipped Sword", false);
                equippedSword = false;
                swordIcon.SetActive(false);
            }

            // Unequip
            if (animator.GetBool("Equipped Bow") == true)
            {
                animator.SetTrigger("Unequip");
                animator.SetBool("Equipped Bow", false);
                equippedBow = false;
                bowIcon.SetActive(false);

            }

            if(animator.GetBool("Equipped Sword") == false && animator.GetBool("Equipped Bow") == false)
            {
                handsIcon.SetActive(true);
            }
            else
            {
                handsIcon.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Equip sword
            if (animator.GetBool("Equipped Sword") == false)
            {
                if(animator.GetBool("Equipped Bow") == true)
                {
                    arms.GetComponent<WeaponInteraction>().DeattachBow();
                    animator.SetBool("Equipped Bow", false);
                    equippedBow = false;
                    bowIcon.SetActive(false);
                }

                handsIcon.SetActive(false);
                animator.SetTrigger("Equip Sword");
                animator.SetBool("Equipped Sword", true);

                FindObjectOfType<AudioManager>().PlayDelayed("SwordEquip", 0.6f);

                equippedSword = true;
                swordIcon.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && hasArrows)
        {
            // Equip Bow
            if (animator.GetBool("Equipped Bow") == false)
            {
                if (animator.GetBool("Equipped Sword") == true)
                {
                    arms.GetComponent<WeaponInteraction>().DeattachSword();
                    animator.SetBool("Equipped Sword", false);
                    equippedSword = false;
                    swordIcon.SetActive(false);
                }

                handsIcon.SetActive(false);
                animator.SetTrigger("Equip Bow");
                animator.SetBool("Equipped Bow", true);
                equippedBow = true;
                bowIcon.SetActive(true);
            }
        }

        // Attack
        if (Input.GetMouseButtonDown(0) && equippedSword && canAttack)
        {
            canAttack = false;
            isAttacking = true;
            animator.SetTrigger("Attack");

            FindObjectOfType<AudioManager>().PlayDelayed("SwordHit", 0.4f);
            FindObjectOfType<AudioManager>().PlayDelayed("QuickAttackVoice", 0.4f);

            Invoke(nameof(ResetAttack), attackCooldownTime);
        }

        // Strong Attack
        if (Input.GetMouseButtonDown(1) && equippedSword && canAttack)
        {
            canAttack = false;
            isAttacking = true;
            animator.SetTrigger("Strong Attack");

            FindObjectOfType<AudioManager>().PlayDelayed("SwordHit", 0.9f);
            FindObjectOfType<AudioManager>().PlayDelayed("StrongAttackVoice", 0.4f);

            Invoke(nameof(ResetAttack), attackCooldownTime);
        }

        // Long Shot
        if (Input.GetMouseButton(1) && equippedBow && canShoot)
        {
            canShoot = false;

            FindObjectOfType<AudioManager>().Play("BowStringLong");

            animator.SetTrigger("Long Shoot");

            holding = true;
        }

        if (Input.GetMouseButtonDown(0) && holding)
        {
            holding = false;

            FindObjectOfType<AudioManager>().Play("BowShot");

            LongShotAnimation();
        }

        if (Input.GetMouseButtonUp(1) && holding)
        {
            holding = false;
            animator.SetTrigger("Release");
            canShoot = true;
        }

        // Shoot
        if (Input.GetMouseButtonDown(0) && equippedBow && canShoot && !holding)
        {
            canShoot = false;

            FindObjectOfType<AudioManager>().Play("BowStringShort");
            FindObjectOfType<AudioManager>().PlayDelayed("BowShot", 0.4f);

            StartCoroutine(ShootAnimation());
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash && canDashWithEnergy)
        {
            canDash = false;

            playerEnergy.energyUsed(100);

            Invoke(nameof(ResetDash), dashCooldownTime);
            dashCurrentTime = 0f;
        }

        if (dashCurrentTime < dashDuration)
        {
            move *= dashDistance;
            controller.Move(move * dashSpeed * Time.deltaTime);
            dashCurrentTime += 0.1f;
        }
        else
        {
            move = Vector3.zero;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void CheckEnergy()
    {
        currentEnergy = playerEnergy.CurrentEnergy;
        if(currentEnergy > 0)
        {
            canSprintWithEnergy = true;
            if (currentEnergy >= 100)
            {
                canDashWithEnergy = true;
            }
            else
            {
                canDashWithEnergy = false;
            }
        }
        else
        {
            canSprintWithEnergy = false;
        }
    }

    void CheckArrows()
    {
        currentArrowCount = arrowSys.CurrentArrows;
        if(currentArrowCount <= 0)
        {
            hasArrows = false;
            unequipBowWithNoArrows();
        }
        else
        {
            hasArrows = true;
        }
    }

    void ResetDash()
    {
        canDash = true;
    }

    void ResetAttack()
    {
        isAttacking = false;
        canAttack = true;
    }

    void ResetShoot()
    {
        arms.GetComponent<WeaponInteraction>().AttachArrow();
        canShoot = true;
    }

    void unequipBowWithNoArrows()
    {
        // Equip sword
        if (animator.GetBool("Equipped Sword") == false)
        {
            if (animator.GetBool("Equipped Bow") == true)
            {
                arms.GetComponent<WeaponInteraction>().DeattachBow();
                animator.SetBool("Equipped Bow", false);
                equippedBow = false;
                bowIcon.SetActive(false);
            }

            handsIcon.SetActive(false);
            animator.SetTrigger("Equip Sword");
            animator.SetBool("Equipped Sword", true);
            equippedSword = true;
            swordIcon.SetActive(true);
        }
    }

    IEnumerator ShootAnimation()
    {
        animator.SetTrigger("Quick Shoot");
        arrowSys.arrowsUsed(1);

        yield return new WaitForSeconds(0.4f);

        arms.GetComponent<WeaponInteraction>().DeattachArrow();

        Vector3 targetPoint;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        canShoot = false;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        GameObject currentArrow = Instantiate(arrow, attackPoint.position, Quaternion.identity);
        currentArrow.SetActive(true);
        currentArrow.name = "Shot Arrow";
        currentArrow.transform.forward = directionWithoutSpread.normalized;
        currentArrow.transform.Rotate(-90f, 0, 0);
        currentArrow.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        currentArrow.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
        Destroy(currentArrow, 5f);

        Invoke(nameof(ResetShoot), shootCooldownTime);
    }

    void LongShotAnimation()
    {
        arrowSys.arrowsUsed(1);
        animator.SetTrigger("Arrow Shot");
        arms.GetComponent<WeaponInteraction>().DeattachArrow();

        Vector3 targetPoint;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        canShoot = false;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        GameObject currentArrow = Instantiate(arrow, attackPoint.position, Quaternion.identity);
        currentArrow.SetActive(true);
        currentArrow.name = "Shot Arrow";
        currentArrow.transform.forward = directionWithoutSpread.normalized;
        currentArrow.transform.Rotate(-90f, 0, 0);
        currentArrow.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        currentArrow.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
        Destroy(currentArrow, 5f);

        Invoke(nameof(ResetShoot), shootCooldownTime);
    }
}
