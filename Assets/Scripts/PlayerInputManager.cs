using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;

    [SerializeField] Transform tester;
    [SerializeField] TextMeshPro testText;

    private GravityAffectedMovement gravityAffectedMovement;
    private GravityAffected gravityAffected;

    InputAction.CallbackContext? callbackContext;

    private void Start()
    {
        gravityAffectedMovement = GetComponent<GravityAffectedMovement>();
        gravityAffected = GetComponent<GravityAffected>();
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
        if (callbackContext != null)
        {
            Vector3? gravityDir = gravityAffected.GetGravityDirection();

            if (gravityDir != null)
            {
                cameraTarget.transform.up = (Vector3) gravityDir;
            }
        }
    }

    private void HandleMove ()
    {
        if (callbackContext != null)
        {
            InputAction.CallbackContext safeContext = (InputAction.CallbackContext)callbackContext;
            Vector3? gravityDir = gravityAffected.GetGravityDirection();

            Vector2 dir = safeContext.ReadValue<Vector2>();

            //Vector3 camForward = Camera.main.transform.forward;
            //float x = Vector3.Dot(camForward, transform.right);
            //float y = Vector3.Dot(camForward, transform.forward);

            //Vector2 cameraVec = new Vector2(x, y).normalized;

            if (gravityDir != null)
            {
                Quaternion rotationFromCameraUpToGravityUp = Quaternion.FromToRotation(Camera.main.transform.up, (Vector3)gravityDir);
                Vector3 adjustedCameraForward = rotationFromCameraUpToGravityUp * Camera.main.transform.forward;

                Debug.DrawRay(this.transform.position, adjustedCameraForward * 3f, Color.cyan);
                Debug.DrawRay(this.transform.position, (Vector3)gravityDir * 3f, Color.cyan);

                float inputAsAngleDegrees = Mathf.Rad2Deg * Mathf.Atan(dir.x / dir.y);

                Debug.Log("" + dir.x + " / " + dir.y + " = " + (dir.x / dir.y));
                Debug.Log("arctan(" + dir.x / dir.y + ") = " + Mathf.Atan(dir.x / dir.y) + " or " + inputAsAngleDegrees);

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

