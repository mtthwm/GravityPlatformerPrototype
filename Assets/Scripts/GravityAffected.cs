using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAffected : MonoBehaviour
{
    List<GravityField> fields = new List<GravityField>();
    Rigidbody rb;

    public void AddAffectingField (GravityField field)
    {
        fields.Add(field);
    }

    public void RemoveAffectingField (GravityField field)
    {
        fields.Remove(field);
    }

    /// <summary>
    /// Gets the direction that the active gravity field is exerting a force in.
    /// </summary>
    /// <returns>The normalized vector.</returns>
    public Vector3? GetGravityDirection()
    {
        Debug.Log("GetGravityDirection! " + Utils.ListToString<GravityField>(fields));
        if (fields.Count > 0)
        {
            GravityField currentField = fields[0];
            Vector3 dir = currentField.getGravityDirection(this.transform.position);
            return dir;
        }

        return null;
    }

    private void OnValidate()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}
