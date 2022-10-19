using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

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
    [SerializeField] private float attackCooldownTime = 0.45f;

    [SerializeField] private CharacterController controller;
    private float horizontal;
    private float vertical;

    [Header("Walk")]
    [SerializeField] private float walkSpeed = 3.2f;
    private Vector3 move;

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
    //GameObject currentArrow;

    //[SerializeField] private GameObject rightHand;

    private void Start()
    {
        animator = GetComponentInChildren(typeof(Animator)) as Animator;
        sword.SetActive(false);
        bow.SetActive(false);
        arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Ground check
        onGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (onGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        PlayerInput();

        MovePlayer();

    }

    private void PlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        move = transform.right * horizontal + transform.forward * vertical;

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            animator.SetBool("Walk", true);

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("Sprint", false);
            }
        }

        if ((Mathf.Abs(horizontal) == 0) && (Mathf.Abs(vertical) == 0))
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Sprint", false);
        }

        controller.Move(move * walkSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && onGround)
        {
            if (animator.GetBool("Equipped Sword") == false && animator.GetBool("Equipped Bow") == false)
            {
                animator.SetTrigger("Jump");
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -1 * gravity);
        }

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
            {
                animator.SetBool("Sprint", true);
            }

            controller.Move(move * sprintSpeed * Time.deltaTime);
        }

        // Weapon
        if (Input.GetKeyDown(KeyCode.E))
        {
            //// Equip
            //if (animator.GetBool("Equipped") == false)
            //{
            //    animator.SetTrigger("Equip");
            //    animator.SetBool("Equipped", true);
            //    equipped = true;
            //}
            //// Unequip
            //else
            //{
            //    animator.SetTrigger("Unequip");
            //    animator.SetBool("Equipped", false);
            //    equipped = false;
            //}

            // Unequip
            if (animator.GetBool("Equipped Sword") == true)
            {
                animator.SetTrigger("Unequip");
                animator.SetBool("Equipped Sword", false);
                equippedSword = false;
            }

            // Unequip
            if (animator.GetBool("Equipped Bow") == true)
            {
                animator.SetTrigger("Unequip");
                animator.SetBool("Equipped Bow", false);
                equippedBow = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Equip
            if (animator.GetBool("Equipped Sword") == false)
            {
                if(animator.GetBool("Equipped Bow") == true)
                {
                    arms.GetComponent<WeaponInteraction>().DeattachBow();
                    animator.SetBool("Equipped Bow", false);
                    equippedBow = false;
                }

                animator.SetTrigger("Equip Sword");
                animator.SetBool("Equipped Sword", true);
                equippedSword = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Equip Bow
            if (animator.GetBool("Equipped Bow") == false)
            {
                if (animator.GetBool("Equipped Sword") == true)
                {
                    arms.GetComponent<WeaponInteraction>().DeattachSword();
                    animator.SetBool("Equipped Sword", false);
                    equippedSword = false;
                }

                animator.SetTrigger("Equip Bow");
                animator.SetBool("Equipped Bow", true);
                equippedBow = true;
                //currentArrow = Instantiate(arrow, attackPoint.position, Quaternion.identity);
            }
        }

        // Attack
        if (Input.GetMouseButtonDown(0) && equippedSword && canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
            Invoke(nameof(ResetAttack), attackCooldownTime);
        }

        // Strong Attack
        if (Input.GetMouseButtonDown(1) && equippedSword && canAttack)
        {
            canAttack = false;
            animator.SetTrigger("Strong Attack");
            Invoke(nameof(ResetAttack), attackCooldownTime);
        }

        // Shoot
        if (Input.GetMouseButtonDown(0) && equippedBow && canShoot)
        {
            //Vector3 targetPoint;
            //Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            canShoot = false;
            //RaycastHit hit;
            //if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            //{
            //    targetPoint = hit.point;
            //}
            //else
            //{
            //    targetPoint = ray.GetPoint(75);
            //}

            StartCoroutine(ShootAnimation());

            //Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            //GameObject currentArrow = Instantiate(arrow, attackPoint.position, Quaternion.identity);
            //currentArrow.transform.forward = directionWithoutSpread.normalized;
            //currentArrow.transform.Rotate(-90f, 0, 0);
            //currentArrow.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
            //currentArrow.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
            //Destroy(currentArrow, 5f);


            //Invoke(nameof(ResetShoot), shootCooldownTime);
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash)
        {
            canDash = false;
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

    void ResetDash()
    {
        canDash = true;
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void ResetShoot()
    {
        arms.GetComponent<WeaponInteraction>().AttachArrow();
        //currentArrow = Instantiate(arrow, attackPoint.position, Quaternion.identity);
        //currentArrow.transform.parent = rightHand.transform;
        canShoot = true;
    }

    IEnumerator ShootAnimation()
    {
        animator.SetTrigger("Quick Shoot");
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
        currentArrow.transform.forward = directionWithoutSpread.normalized;
        currentArrow.transform.Rotate(-90f, 0, 0);
        currentArrow.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);
        currentArrow.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
        Destroy(currentArrow, 5f);

        Invoke(nameof(ResetShoot), shootCooldownTime);
    }
}
