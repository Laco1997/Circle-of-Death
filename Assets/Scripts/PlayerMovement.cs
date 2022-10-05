using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

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

    private void Start()
    {
        animator = GetComponentInChildren(typeof(Animator)) as Animator;
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
            if (animator.GetBool("Equipped") == false)
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
            // Equip
            if (animator.GetBool("Equipped") == false)
            {
                animator.SetTrigger("Equip");
                animator.SetBool("Equipped", true);
            }
            // Unequip
            else
            {
                animator.SetTrigger("Unequip");
                animator.SetBool("Equipped", false);
            }
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

}
