using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private GravityAffected gravityAffected;

    InputAction.CallbackContext? callbackContext;

    private void Start()
    {
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
        Debug.Log("HandleJump");
        if (enabled)
        {
            if (value.performed)
            {
                gravityAffected.Jump();
            }
        }
    }

    private void Update()
    {
        if (callbackContext != null)
        {
            InputAction.CallbackContext safeContext = (InputAction.CallbackContext)callbackContext;
            gravityAffected.Move(safeContext.ReadValue<Vector2>());
        }
    }
}

