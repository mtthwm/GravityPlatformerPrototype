using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGravityField : GravityField
{
    /// <summary>
    /// Gets the gravity direction in a normalized vector.
    /// </summary>
    /// <param name="otherPosition">The position of the other object.</param>
    /// <returns></returns>

    public override Vector3 getGravityDirection(Vector3 otherPosition)
    {
        return (otherPosition - this.transform.position).normalized;
    }
}
