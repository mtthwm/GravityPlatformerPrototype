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

    [SerializeField] Transform movementCameraTarget;
    [SerializeField] Transform shoulderCameraTarget;
    [SerializeField] CinemachineVirtualCamera shoulderCamera;

    private GravityAffectedMovement gravityAffectedMovement;
    private GravityAffected gravityAffected;
    private GunManager gunManager;
    private bool isShooting;
    private bool isScoped;

    InputAction.CallbackContext? callbackContext;

    private void Start()
    {
        gravityAffectedMovement = GetComponent<GravityAffectedMovement>();
        gravityAffected = GetComponent<GravityAffected>();
        gunManager = GetComponent<GunManager>();
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
            // Turn to face the proper direction
            Vector3? gravityDir = gravityAffected.GetGravityDirection();
            if (gravityDir != null)
            {
                gravityAffectedMovement.SetFacingDirection(GetAdjustedCameraForward((Vector3)gravityDir));
            }

            isScoped = true;

            // Switch to shoulderCamera and broadcast the scope event
            shoulderCamera.Priority = 10;
            OnToggleScope?.Invoke(true);
        }
        else if (value.canceled)
        {
            isScoped = false;
            shoulderCamera.Priority = 0;
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
        UpdateMovementCameraTarget();
        UpdateShoulderCameraTarget();

        if (isShooting)
        {
            Vector3? gravityDir = gravityAffected.GetGravityDirection();
            if (gravityDir != null)
            {
                gravityAffectedMovement.SetFacingDirection(GetAdjustedCameraForward((Vector3)gravityDir));
            }
        }
    }

    private void UpdateMovementCameraTarget ()
    {
        Vector3? gravityDir = gravityAffected.GetGravityDirection();

        if (gravityDir != null)
        {
            movementCameraTarget.transform.up = (Vector3)gravityDir;
        }
    }

    private void UpdateShoulderCameraTarget ()
    {
        Vector3? gravityDir = gravityAffected.GetGravityDirection();

        if (gravityDir != null)
        {
            shoulderCameraTarget.transform.forward = GetAdjustedCameraForward((Vector3)gravityDir).normalized;
            //shoulderCameraTarget.transform.up = (Vector3)gravityDir;
        }
    }

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
            Vector3? gravityDir = gravityAffected.GetGravityDirection();

            Vector2 dir = safeContext.ReadValue<Vector2>();

            if (gravityDir != null)
            {
                Vector3 adjustedCameraForward = GetAdjustedCameraForward((Vector3)gravityDir);

                float inputAsAngleDegrees = Mathf.Rad2Deg * Mathf.Atan(dir.x / dir.y);

                Quaternion necessaryRotation = Quaternion.AngleAxis(inputAsAngleDegrees, (Vector3)gravityDir);

                Vector3 realMoveDir = necessaryRotation * adjustedCameraForward;

                gravityAffectedMovement.Move(Utils.IsPointingDown(dir) * realMoveDir);
            }
        }
        else
        {
            gravityAffectedMovement.EndMove();
        }

    }
}

