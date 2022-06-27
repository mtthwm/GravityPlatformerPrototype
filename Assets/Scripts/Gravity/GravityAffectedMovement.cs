using UnityEngine;

public class GravityAffectedMovement : MonoBehaviour
{
    [SerializeField] float movementAccelleration = 5f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float gravityForce = -9.8f;
    [SerializeField] float gravityPauseTime = 0.4f;
    [SerializeField] float maxMovementVelocity = 10f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] GameObject rig;

    Vector3 moveVelocity;
    Vector3 gravityVelocity;
    private float lastJump;
    Vector3 facingDir;
    GravityAffected gravityAffected;
    Rigidbody rb;
    Vector3 accelleration;

    /// <summary>
    /// Directly moves an object in the specified direction. USE WITH CAUTION!!
    /// </summary>
    /// <param name="dir">The direction.</param>
    public void Move(Vector3 dir)
    {
        facingDir = dir;

        //Check if the velocity in the movement direction is within the threshold
        if (Vector3.Dot(rb.velocity, dir) < maxMovementVelocity)
        {
            rb.AddForce(dir * movementAccelleration);
        }
    }

    /// <summary>
    /// Moves in a 2D Direction "Wrapping" around the gravity field.
    /// </summary>
    /// <param name="dir">The 2D Direction.</param>
    public void Move(Vector2 dir)
    {
        Vector3 gravityDir = gravityAffected.GetGravityDirection();

        Vector3 realMoveDir = Utils.Align2DVector(gravityDir, dir);
        Move(realMoveDir);
    }

    public void EndMove()
    {
        moveVelocity = Vector3.zero;
    }

    public void Jump()
    {
        Vector3 dir = gravityAffected.GetGravityDirection();
        if (IsGrounded())
        {
            rb.AddForce(dir * jumpForce);
            lastJump = Time.time;
        }
    }

    public Vector3 GetFacingDirection()
    {
        return facingDir;
    }

    /// <summary>
    /// Sets the facing direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <returns>The time that it will take to rotate</returns>
    public void SetFacingDirection (Vector3 direction)
    {
        facingDir = direction;
    }

    public void Accellerate (Vector3 accelleration)
    {
        //this.accelleration = accelleration;
    }

    public void EndAccelleration ()
    {
        //accelleration = Vector3.zero;
        //rb.velocity = Vector3.zero;
    }

    private void Start()
    {
        facingDir = rig.transform.forward;
        gravityAffected = GetComponent<GravityAffected>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RotateFacing();
        HandleGravity();
    }

    private void HandleGravity ()
    {
        rb.AddForce(gravityAffected.GetGravityDirection() * gravityForce);
    }

    /// <summary>
    /// [Deprecated] A remnant of the former movement system
    /// </summary>
    private void HandleVelocity()
    {
        if (accelleration == Vector3.zero)
        {
            rb.velocity = moveVelocity + gravityVelocity;
            if (Time.time - lastJump >= gravityPauseTime)
            {
                gravityVelocity = (Vector3)gravityAffected.GetGravityDirection() * gravityForce;
            }
        }
        else
        {
            rb.velocity += accelleration;
        }
    }

    private void RotateFacing()
    {
        Vector3 dir = gravityAffected.GetGravityDirection();
        rig.transform.rotation = Quaternion.RotateTowards(Quaternion.LookRotation(rig.transform.forward, rig.transform.up), Quaternion.LookRotation(facingDir, dir), rotationSpeed);
    }

    private bool IsGrounded ()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayers);
    }
}
