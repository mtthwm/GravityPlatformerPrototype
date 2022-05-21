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

            //Rotate the input vector to reflect the camera's orientation
            dir = Quaternion.AngleAxis(-Camera.main.transform.rotation.eulerAngles.y, Vector3.forward) * dir;

            Debug.DrawRay(tester.position, dir * 2f, Color.red);
            testText.text = "" + -Camera.main.transform.rotation.eulerAngles.y;

            if (gravityDir != null)
            {
                Vector3 realMoveDir = Utils.Align2DVector((Vector3)gravityDir, dir);
                gravityAffectedMovement.Move(realMoveDir);
            }
        }
        else
        {
            gravityAffectedMovement.EndMove();
        }

    }
}

