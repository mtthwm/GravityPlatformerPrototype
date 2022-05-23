using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : Gun
{
    [SerializeField] GravityAffectedMovement gravityMovement;
    [SerializeField] float pullAccelleration = 5f;

    protected override void ResolveHit (RaycastHit hit)
    {
        gravityMovement.Accellerate((hit.point - gravityMovement.transform.position).normalized * pullAccelleration);
    }

    public override void EndShoot ()
    {
        base.EndShoot();
        gravityMovement.EndAccelleration();
    }
}
