using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAffected : MonoBehaviour
{
    LinkedList<GravityField> fields = new LinkedList<GravityField>();
    Rigidbody rb;

    public void AddAffectingField (GravityField field)
    {
        fields.AddLast(field);
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
        if (fields.Count > 0)
        {
            GravityField currentField = fields.First.Value;
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
