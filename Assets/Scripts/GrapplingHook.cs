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

    protected override void ResolveHit (RaycastHit hit)
    {
        OnGrappleStart?.Invoke();
        gravityMovement.Accellerate((hit.point - gravityMovement.transform.position).normalized * pullAccelleration);
    }

    public override void EndShoot ()
    {
        OnGrappleEnd?.Invoke();
        base.EndShoot();
        gravityMovement.EndAccelleration();
    }
}
