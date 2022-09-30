using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 6f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool onGround;

    float horizontal;
    float vertical;
    Vector3 move;

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

        controller.Move(move * walkSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && onGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Sprint
        if(Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * sprintSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
