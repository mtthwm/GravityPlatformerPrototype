using UnityEngine;

public class GravityAffectedMovement : MonoBehaviour
{
    [Header("Standard Movement")]
    [SerializeField] float movementAccelleration = 5f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float gravityPauseTime = 0.4f;
    [SerializeField] float maxMovementVelocity = 10f;
    [SerializeField] float gravityForce = -9.8f;

    [Header("Zero G Movement")]
    [SerializeField] float thrusterAccelleration = 5f;

    [Header("General")]
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayers;
    [SerializeField] GameObject rig;

    Vector3 facingDir;
    GravityAffected gravityAffected;
    Rigidbody rb;
    float cachedDrag;

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

    public void Jump()
    {
        Vector3 dir = gravityAffected.GetGravityDirection();
        if (IsGrounded())
        {
            rb.AddForce(dir * jumpForce);
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
        cachedDrag = rb.drag;
    }

    private void FixedUpdate()
    {
        RotateFacing();
        HandleGravity();
    }

    /// <summary>
    /// Applies force due to gravity, as well as handles toggling drag
    /// </summary>
    private void HandleGravity ()
    {
        Vector3 dir = gravityAffected.GetGravityDirection();
        rb.AddForce(dir * gravityForce);

        if (dir == Vector3.zero)
        {
            rb.drag = 0;
        }
        else
        {
            rb.drag = cachedDrag;
        }
    }

    private void RotateFacing()
    {
        Vector3 dir = gravityAffected.GetGravityDirection();
        if (dir != Vector3.zero)
        {
            rig.transform.rotation = Quaternion.RotateTowards(Quaternion.LookRotation(rig.transform.forward, rig.transform.up), Quaternion.LookRotation(facingDir, dir), rotationSpeed);
        }
    }

    private bool IsGrounded ()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayers);
    }
}
