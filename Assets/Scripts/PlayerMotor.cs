using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float acceleration = 1f;
    public float deceleration = 0.3f;
    public float maxSpeed = 20f;
    public float gravity = -9.8f;
    public float turnSpeed = 3f;
    public float minSpeedToTurn = 1f;

    private const float BACKWARDS_SPEED_DIVIDER = 4f;

    private CharacterController playerController;
    private Vector3 velocity;
    private bool isGrounded;
    private float currentTurnAngle = 0f;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckIfGrounded();
    }

    /// <summary>
    /// Process the input to move the player.
    /// </summary>
    /// <param name="input">The input received from the player</param>
    public void ProcessMove(Vector2 input)
    {
        MovePlayer(input);
        TurnPlayer(input);
    }

    /// <summary>
    /// Set <see cref="isGrounded"/> to true if the player is grounded, else set it to false.
    /// </summary>
    private void CheckIfGrounded()
    {
        isGrounded = playerController.isGrounded;
    }

    /// <summary>
    /// Move the player according to the input and applies gravity if the player is in the air.
    /// </summary>
    /// <param name="input"></param>
    private void MovePlayer(Vector2 input)
    {
        // acceleration
        if (input.y != 0)
        {
            velocity.z += acceleration * input.y;
            if (velocity.z > 0)
            {
                velocity.z = Mathf.Min(velocity.z, maxSpeed);
            }
            else
            {
                // go slower when going backwards
                velocity.z = Mathf.Max(velocity.z, -maxSpeed / BACKWARDS_SPEED_DIVIDER);
            }
        }
        // deceleration
        else if (velocity.z > 0)
        {
            velocity.z -= deceleration;
            velocity.z = Mathf.Max(velocity.z, 0);
        }
        else if (velocity.z < 0)
        {
            // slow down faster when going backwards
            velocity.z += deceleration * BACKWARDS_SPEED_DIVIDER;
            velocity.z = Mathf.Min(velocity.z, 0);
        }

        // apply gravity
        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // apply velocity to controller, and adjust for the player's rotation
        playerController.Move(transform.rotation * velocity * Time.deltaTime);
    }

    /// <summary>
    /// Rotate the player according to the input.
    /// </summary>
    /// <param name="input"></param>
    private void TurnPlayer(Vector2 input)
    {
        if (velocity.z > minSpeedToTurn)
        {
            currentTurnAngle = turnSpeed * input.x / (velocity.z * 0.05f);
        }
        else if (velocity.z < 0 && velocity.z < -minSpeedToTurn / BACKWARDS_SPEED_DIVIDER)
        {
            currentTurnAngle = turnSpeed * -input.x;
        }
        else
        {
            currentTurnAngle = 0f;
        }

        transform.Rotate(new Vector3(0, currentTurnAngle, 0));
    }
}
