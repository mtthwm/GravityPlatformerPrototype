using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : Gun
{
    public delegate void GrappleStartAction();
    public static event GrappleStartAction OnGrappleStart;
    public delegate void GrappleEndAction();
    public static event GrappleEndAction OnGrappleEnd;

    [SerializeField] GravityAffectedMovement gravityMovement;
    [SerializeField] float pullAccelleration = 5f;
    bool canGrapple = true;

    protected override void ResolveHit (RaycastHit hit)
    {
        if (canGrapple)
        {
            gravityMovement.Accellerate((hit.point - gravityMovement.transform.position).normalized * pullAccelleration);
        }
    }

    public override void BeginShoot()
    {
        OnGrappleStart?.Invoke();
        base.BeginShoot();
    }

    public override void EndShoot ()
    {
        OnGrappleEnd?.Invoke();
        base.EndShoot();
        gravityMovement.EndAccelleration();
    }

    private void HandleCrash ()
    {
        if (isShooting)
        {
            EndShoot();
        }
    }

    private void OnEnable()
    {
        CrashManager.OnCrash += HandleCrash;
    }

    private void OnDisable()
    {
        CrashManager.OnCrash -= HandleCrash;
    }
}
