using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float slopeLimit;
    private RaycastHit slopeHit;
    public float centerX;
    public float centerY;
    public float centerZ;
    public float colliderWidth;
    public float colliderHeight;
    public float colliderLength;
    public bool isGrounded;

    private Vector3 velocity;
    private BoxCollider collider;
    

    void Start() {
        collider = GetComponent<BoxCollider>();
        colliderWidth = collider.size.x;
        colliderHeight = collider.size.y;
        colliderLength = collider.size.z;
    }

    public void FixedUpdate() {
        CheckIfGrounded();
    }

    public void Update() {
        CheckIfGrounded();
    }


    public void Move(Vector3 move) {
        velocity = move;
        float speed = velocity.z;
        
        if (onSlope()) {
            velocity = slopeMoveDirection(velocity);
            velocity *= speed;
            velocity = limitSpeed(velocity, speed);
        }

        Debug.Log("Move: " + velocity.x + " " + velocity.y + " " + velocity.z);

        transform.position += velocity;
    }

    private void CheckIfGrounded() {
        Vector3 front = transform.position + transform.forward * (colliderLength / 2);
        Vector3 back = transform.position - transform.forward * (colliderLength / 2);
        if (Physics.Raycast(transform.position, Vector3.down, colliderHeight /2 + 0.5f)
            || Physics.Raycast(front, Vector3.down, colliderHeight / 2 + 0.5f)
            || Physics.Raycast(back, Vector3.down, colliderHeight / 2 + 0.5f)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }

    private Vector3 limitSpeed(Vector3 move, float speed) {
        move = move * speed;
        return move;
    }

    private bool onSlope() {
        Vector3 front = transform.position + transform.forward * (colliderLength / 2);
        Vector3 back = transform.position - transform.forward * (colliderLength / 2);
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, colliderHeight /2 + 0.5f)
            || Physics.Raycast(front, Vector3.down, out slopeHit, colliderHeight / 2 + 0.1f)
            || Physics.Raycast(back, Vector3.down, out slopeHit, colliderHeight / 2 + 0.1f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < slopeLimit && angle != 0;
        }
        return false;
    }

    private Vector3 slopeMoveDirection(Vector3 move) {
        return Vector3.ProjectOnPlane(move.normalized, slopeHit.normal).normalized;
    }



}