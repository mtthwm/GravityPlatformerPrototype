using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineFreeLook))]
public class PlayerMovementCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera grapplingHookCamera;

    CinemachineFreeLook camera;

    private void Start()
    {
        camera = GetComponent<CinemachineFreeLook>();
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

    private void LockCameraRotation()
    {
        grapplingHookCamera.transform.position = Camera.main.transform.position;
        grapplingHookCamera.transform.rotation = Camera.main.transform.rotation;
        grapplingHookCamera.Priority = 1000;
    }

    private void UnlockCameraRotation ()
    {
        StartCoroutine(DelaySwitch());
    }

    private IEnumerator DelaySwitch ()
    {
        //yield return new WaitForSeconds(2f);
        yield return null;
        grapplingHookCamera.Priority = 0;
    }
}
