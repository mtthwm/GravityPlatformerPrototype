using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Essentially "aligns" a 2D vector with a normal vector. If dir is a 2D vector on a plane, that plane is perpendicular to normal.
    /// </summary>
    /// <param name="normal">The normal vector.</param>
    /// <param name="dir">The direction.</param>
    /// <returns></returns>
    public static Vector3 Align2DVector(Vector3 normal, Vector2 dir)
    {
        Vector3 dir3 = new Vector3(dir.x, 0, dir.y);
        //Quaternion rotation = Quaternion.LookRotation(forward, normal);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        return rotation * dir3;
    }
}
