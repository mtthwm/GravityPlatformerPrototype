using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashManager : MonoBehaviour
{
    public delegate void CrashAction();
    public static event CrashAction OnCrash;

    [SerializeField] float velocityThreshold = 5;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude >= velocityThreshold)
        {
            OnCrash?.Invoke();
        }
    }
}
