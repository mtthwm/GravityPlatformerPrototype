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
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayers;

    Vector3 moveVelocity;
    Vector3 gravityVelocity;
    private float lastJump;
    Vector3 facingDir;
    GravityAffected gravityAffected;
    Rigidbody rb;

    /// <summary>
    /// Directly moves an object in the specified direction. USE WITH CAUTION!!
    /// </summary>
    /// <param name="dir">The direction.</param>
    public void Move(Vector3 dir)
    {
        facingDir = dir;
        moveVelocity = dir * movementSpeed;
    }

    /// <summary>
    /// Moves in a 2D Direction "Wrapping" around the gravity field.
    /// </summary>
    /// <param name="dir">The 2D Direction.</param>
    public void Move(Vector2 dir)
    {
        Vector3? gravityDir = gravityAffected.GetGravityDirection();

        if (gravityDir != null)
        {
            Vector3 realMoveDir = Utils.Align2DVector((Vector3)gravityDir, dir);
            
            Move(realMoveDir);
        }
    }

    public void EndMove()
    {
        moveVelocity = Vector3.zero;
    }

    public void Jump()
    {
        Vector3? dir = gravityAffected.GetGravityDirection();
        if (dir != null && IsGrounded())
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

    private bool IsGrounded ()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayers);
    }
}
