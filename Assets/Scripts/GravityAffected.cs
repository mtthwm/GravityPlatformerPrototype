using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAffected : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float gravityForce = -9.8f;

    LinkedList<GravityField> fields = new LinkedList<GravityField>();
    Rigidbody rb;
    Vector3 facingDir;

    Vector3 moveVelocity;
    Vector3 jumpVelocity;
    Vector3 gravityVelocity;

    public void Move (Vector2 dir)
    {
        Vector3 dir3 = new Vector3(dir.x, 0, dir.y);
        Vector3? gravityDir = GetGravityDirection();

        if (gravityDir != null)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, (Vector3) gravityDir);
            Vector3 realMoveDir = rotation * dir3;
            facingDir = realMoveDir;
            moveVelocity = realMoveDir * movementSpeed;
        }
    }

    public void Jump ()
    {
        jumpVelocity = (Vector3) GetGravityDirection() * jumpForce * Time.deltaTime;
    }

    public void AddAffectingField (GravityField field)
    {
        fields.AddLast(field);
    }

    public void RemoveAffectingField (GravityField field)
    {
        fields.Remove(field);
    }

    private void Start()
    {
        facingDir = transform.forward;
    }

    private void OnValidate()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        RotateFacing();
        Vector3? dir = GetGravityDirection();
        if (dir != null)
        {
            Debug.Log(moveVelocity + " " + jumpVelocity + " " + gravityVelocity);
            gravityVelocity = (Vector3)GetGravityDirection() * gravityForce;
            rb.velocity = moveVelocity + jumpVelocity + gravityVelocity;
        }
        
    }

    /// <summary>
    /// Adds a force in the direction of the active gravity field.
    /// </summary>
    /// <returns></returns>
    private void ExertForce ()
    {
        Vector3? dir = GetGravityDirection();
        if (dir != null)
        {
            rb.AddForce((Vector3) dir * Physics.gravity.y);
        }
    }

    /// <summary>
    /// Rotates this object so that it is aligned with the gravity field.
    /// </summary>
    /// <returns></returns>
    private void RotateNormal()
    {
        Vector3? dir = GetGravityDirection();
        if (dir != null)
        {
            transform.up = (Vector3) dir;
        }
    }

    private void RotateFacing()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(facingDir), rotationSpeed);
    }

    /// <summary>
    /// Gets the direction that the active gravity field is exerting a force in.
    /// </summary>
    /// <returns>The normalized vector.</returns>
    private Vector3? GetGravityDirection ()
    {
        if (fields.Count > 0)
        {
            GravityField currentField = fields.First.Value;
            Vector3 dir = currentField.getGravityDirection(this.transform.position);
            return dir;
        }

        return null;
    }
}
