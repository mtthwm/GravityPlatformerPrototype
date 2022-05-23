using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;

    private GravityAffectedMovement gravityAffectedMovement;
    private GravityAffected gravityAffected;
    private GunManager gunManager;

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
        Vector3? gravityDir = gravityAffected.GetGravityDirection();
        if (gravityDir != null)
        {
            gravityAffectedMovement.SetFacingDirection(GetAdjustedCameraForward((Vector3)gravityDir));
        }

        if (value.performed)
        {
            gunManager.BeginShootPrimary();
        }
        else if (value.canceled)
        {
            gunManager.EndShootPrimary();
        }
    }

    public void ToggleShootSecondary(InputAction.CallbackContext value)
    {
        Vector3? gravityDir = gravityAffected.GetGravityDirection();
        if (gravityDir != null)
        {
            gravityAffectedMovement.SetFacingDirection(GetAdjustedCameraForward((Vector3)gravityDir));
        }

        if (value.performed)
        {
            gunManager.BeginShootSecondary();
        }
        else if (value.canceled)
        {
            gunManager.EndShootSecondary();
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
        UpdateCameraTarget();
    }

    private void UpdateCameraTarget ()
    {
        Vector3? gravityDir = gravityAffected.GetGravityDirection();

        if (gravityDir != null)
        {
            cameraTarget.transform.up = (Vector3) gravityDir;
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

