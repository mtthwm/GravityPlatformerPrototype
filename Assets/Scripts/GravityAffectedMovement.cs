using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAffectedMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float gravityForce = -9.8f;
    [SerializeField] float gravityPauseTime = 0.4f;

    Vector3 moveVelocity;
    Vector3 gravityVelocity;
    private float lastJump;
    Vector3 facingDir;
    GravityAffected gravityAffected;
    Rigidbody rb;

    public void Move(Vector2 dir)
    {
        Vector3 dir3 = new Vector3(dir.x, 0, dir.y);
        Vector3? gravityDir = gravityAffected.GetGravityDirection();

        if (gravityDir != null)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, (Vector3)gravityDir);
            Vector3 realMoveDir = rotation * dir3;
            facingDir = realMoveDir;
            moveVelocity = realMoveDir * movementSpeed;
        }
    }

    public void EndMove()
    {
        moveVelocity = Vector3.zero;
    }

    public void Jump()
    {
        Vector3? dir = gravityAffected.GetGravityDirection();
        if (dir != null)
        {
            gravityVelocity += (Vector3)dir * jumpForce;
            lastJump = Time.time;
        }
    }

    private void Start()
    {
        facingDir = transform.forward;
        gravityAffected = GetComponent<GravityAffected>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RotateFacing();
        HandleVelocity();
    }

    private void HandleVelocity()
    {
        Vector3? dir = gravityAffected.GetGravityDirection();
        if (dir != null)
        {
            rb.velocity = moveVelocity + gravityVelocity;
            if (Time.time - lastJump >= gravityPauseTime)
            {
                gravityVelocity = (Vector3)gravityAffected.GetGravityDirection() * gravityForce;
            }
        }
    }

    private void RotateFacing()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(facingDir), rotationSpeed);
    }
}
