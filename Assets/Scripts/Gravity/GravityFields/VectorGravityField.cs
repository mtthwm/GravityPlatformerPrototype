using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorGravityField : GravityField
{
    [SerializeField] Vector3 direction = new Vector3(0, 1, 0);
    /// <summary>
    /// Gets the gravity direction in a normalized vector.
    /// </summary>
    /// <param name="otherPosition">The position of the other object.</param>
    /// <returns></returns>

    public override Vector3 getGravityDirection(Vector3 otherPosition)
    {
        return (transform.rotation * direction).normalized;
    }

    public override string ToString()
    {
        return direction.ToString();
    }
}
