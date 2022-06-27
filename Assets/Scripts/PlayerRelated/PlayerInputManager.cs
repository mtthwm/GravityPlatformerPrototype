using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class PlayerInputManager : MonoBehaviour
{
    public delegate void ScopeAction(bool scoped);
    public static event ScopeAction OnToggleScope;
    public enum MovementType
    {
        Normal,
        ZeroG,
    };

    [SerializeField] bool turnToShootDir = false;

    public MovementType movementType { get; private set; }

    private GravityAffectedMovement gravityAffectedMovement;
    private GravityAffected gravityAffected;
    private MouseRotation mouseRotation;
    private GunManager gunManager;
    private bool isShooting;
    private bool isScoped;

    InputAction.CallbackContext? callbackContext;

    private void Start()
    {
        gravityAffectedMovement = GetComponent<GravityAffectedMovement>();
        gravityAffected = GetComponent<GravityAffected>();
        gunManager = GetComponent<GunManager>();
        mouseRotation = GetComponent<MouseRotation>();
    }

    public void HandleMovement(InputAction.CallbackContext value)
    {
        if (enabled)
        {
            if (value.performed)
            {
                callbackContext = value;
            } 
            else
            {
                callbackContext = null;
            }
        }
    }

    public void HandleLook(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            mouseRotation.Rotate(value.ReadValue<Vector2>());
        }
    }

    public void ToggleShootPrimary (InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            isShooting = true;
            gunManager.BeginShootPrimary();
        }
        else if (value.canceled)
        {
            isShooting = false;
            gunManager.EndShootPrimary();
        }
    }

    public void ToggleShootSecondary(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            gunManager.BeginShootSecondary();
            isShooting = true;
        }
        else if (value.canceled)
        {
            isShooting = false;
            gunManager.EndShootSecondary();
        }
    }

    public void ToggleScope (InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (turnToShootDir)
            {
                // Turn to face the proper direction
                Vector3 gravityDir = gravityAffected.GetGravityDirection();
                gravityAffectedMovement.SetFacingDirection(GetAdjustedCameraForward(gravityDir));
            }

            isScoped = true;

            // Switch to shoulderCamera and broadcast the scope event
            OnToggleScope?.Invoke(true);
        }
        else if (value.canceled)
        {
            isScoped = false;
            OnToggleScope?.Invoke(false);
        }
    }

    public void HandleJump (InputAction.CallbackContext value)
    {
        if (enabled)
        {
            if (value.performed)
            {
                gravityAffectedMovement.Jump();
            }
        }
    }

    private void Update()
    {
        HandleMove();

        Vector3 gravityDir = gravityAffected.GetGravityDirection();

        if (isShooting)
        {
            
            gravityAffectedMovement.SetFacingDirection(GetAdjustedCameraForward(gravityDir));
        }

        // Constantly change where the player is facing
        gravityAffectedMovement.SetFacingDirection(GetAdjustedCameraForward(gravityAffected.GetGravityDirection()));
    }

    /// <summary>
    /// Gets a vector that points "forward" given the camera's current rotation and the gravity direction
    /// </summary>
    /// <param name="normal">A normal representing the gravity direction</param>
    /// <returns></returns>
    private Vector3 GetAdjustedCameraForward (Vector3 normal)
    {
        Quaternion rotationFromCameraUpToGravityUp = Quaternion.FromToRotation(Camera.main.transform.up, normal);
        Vector3 adjustedCameraForward = rotationFromCameraUpToGravityUp * Camera.main.transform.forward;
        return adjustedCameraForward;
    }

    private void HandleMove ()
    {
        if (callbackContext != null)
        {
            InputAction.CallbackContext safeContext = (InputAction.CallbackContext)callbackContext;
            Vector3 gravityDir = gravityAffected.GetGravityDirection();

            Vector2 dir = safeContext.ReadValue<Vector2>();

            Vector3 adjustedCameraForward;
            if (gravityDir == Vector3.zero)
            {
                adjustedCameraForward = GetAdjustedCameraForward(gravityDir);
            }
            else
            {
                adjustedCameraForward = Camera.main.transform.forward;
            }

            // Get a representation of the input vector as an angle
            float inputAsAngleDegrees = Mathf.Rad2Deg * Mathf.Atan(dir.x / dir.y);

            // Build a quaternion using the input angle with the gravity up direction as the axis of rotation
            Quaternion necessaryRotation = Quaternion.AngleAxis(inputAsAngleDegrees, gravityDir);

            // Rotate the "forward" by the input angle
            Vector3 realMoveDir = necessaryRotation * adjustedCameraForward;

            gravityAffectedMovement.Move(Utils.IsPointingDown(dir) * realMoveDir);
        }
    }
}

