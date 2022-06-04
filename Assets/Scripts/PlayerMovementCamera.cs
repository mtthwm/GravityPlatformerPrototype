using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementCamera : MonoBehaviour
{
    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        GrapplingHook.OnGrappleStart += LockCameraRotation;
        GrapplingHook.OnGrappleEnd += UnlockCameraRotation;
    }

    private void OnDisable()
    {
        GrapplingHook.OnGrappleStart -= LockCameraRotation;
        GrapplingHook.OnGrappleEnd -= UnlockCameraRotation;
    }

    private void LockCameraRotation ()
    {
 
    }

    private void UnlockCameraRotation ()
    {
        Debug.Log("Unlock");
        //playerInput.actions.FindActionMap("Player").FindAction("Look").Enable();
    }
}
