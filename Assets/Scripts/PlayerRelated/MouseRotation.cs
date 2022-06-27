using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MouseRotation : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform body;
    [Range(0, 1f)] public float XSensitivity;
    [Range(0, 1f)] public float YSensitivity;
    public float LimitYRotation = 45;
    public bool InvertY;
    public bool InvertX;

    private GravityAffected gravityAffected;

    private void Start()
    {
        gravityAffected = GetComponent<GravityAffected>();
    }

    private float CalculateMouseMove (float dy)
    {
        return dy * (InvertY ? 1 : -1) * YSensitivity;
    }

    public void Rotate (Vector2 direction)
    {
        // Vertical rotation is unlimited in Zero G but has limits in normal movement
        Vector3 gravityDir = gravityAffected.GetGravityDirection();
        if (gravityDir == Vector3.zero)
        {
            playerCamera.Rotate(new Vector3(CalculateMouseMove(direction.y), 0, 0));
        }
        else
        {
            Vector3 baselineForward = Vector3.Cross(playerCamera.right, gravityDir);
            Vector3 upperLimitVector = Quaternion.AngleAxis(LimitYRotation, playerCamera.right) * baselineForward;
            Vector3 lowerLimitVector = Quaternion.AngleAxis(-LimitYRotation, playerCamera.right) * baselineForward;

            float upperMaxRotation;
            Quaternion.FromToRotation(playerCamera.forward, upperLimitVector).ToAngleAxis(out upperMaxRotation, out _);
            float lowerMaxRotation;
            Quaternion.FromToRotation(playerCamera.forward, lowerLimitVector).ToAngleAxis(out lowerMaxRotation, out _);

            float realRotation = Mathf.Clamp(CalculateMouseMove(direction.y), -lowerMaxRotation, upperMaxRotation);

            playerCamera.Rotate(new Vector3(realRotation, 0, 0));
            
        }

        // Horizontal rotation always stays the same
        body.Rotate(new Vector3(0, direction.x * (InvertX ? -1 : 1) * XSensitivity, 0));
    }
}
