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

    public void Rotate (Vector2 direction)
    {
        Vector3 baselineForward = Vector3.Cross(playerCamera.right, gravityAffected.GetGravityDirection());
        Vector3 upperLimitVector = Quaternion.AngleAxis(LimitYRotation, playerCamera.right) * baselineForward;
        Vector3 lowerLimitVector = Quaternion.AngleAxis(-LimitYRotation, playerCamera.right) * baselineForward;

        Debug.DrawRay(transform.position, upperLimitVector, Color.red);
        Debug.DrawRay(transform.position, lowerLimitVector, Color.red);

        float upperMaxRotation;
        Quaternion.FromToRotation(playerCamera.forward, upperLimitVector).ToAngleAxis(out upperMaxRotation, out _);
        float lowerMaxRotation;
        Quaternion.FromToRotation(playerCamera.forward, lowerLimitVector).ToAngleAxis(out lowerMaxRotation, out _);

        Debug.Log(lowerMaxRotation + " " + upperMaxRotation);

        float realRotation = Mathf.Clamp(direction.y * (InvertY ? 1 : -1) * YSensitivity, -lowerMaxRotation, upperMaxRotation);

        playerCamera.Rotate(new Vector3(realRotation, 0, 0));
        body.Rotate(new Vector3(0, direction.x * (InvertX ? -1 : 1) * XSensitivity, 0));
    }
}
