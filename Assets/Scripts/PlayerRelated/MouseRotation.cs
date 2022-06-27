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
    public float LimitYRotation = 90;
    public bool InvertY;
    public bool InvertX;

    private GravityAffected gravityAffected;

    private void Start()
    {
        gravityAffected = GetComponent<GravityAffected>();
    }

    public void Rotate (Vector2 direction)
    {
        
        playerCamera.Rotate(new Vector3(direction.y * (InvertY ? 1 : -1) * YSensitivity, 0, 0));
        body.Rotate(new Vector3(0, direction.x * (InvertX ? -1 : 1) * XSensitivity, 0));
    }
}
