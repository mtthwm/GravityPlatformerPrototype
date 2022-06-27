using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAffected : MonoBehaviour
{
    public delegate void EnterGravityFieldAction();
    public event EnterGravityFieldAction OnEnterGravityField;
    public delegate void ExitGravityFieldAction();
    public event ExitGravityFieldAction OnExitGravityField;

    List<GravityField> fields = new List<GravityField>();
    Rigidbody rb;

    public void AddAffectingField (GravityField field)
    {
        if (fields.Count == 0)
        {
            OnEnterGravityField?.Invoke();
        }
        fields.Add(field);
    }

    public void RemoveAffectingField (GravityField field)
    {
        if (fields.Count == 1)
        {
            OnExitGravityField?.Invoke();
        }
        fields.Remove(field);
    }

    /// <summary>
    /// Gets the direction that the active gravity field is exerting a force in.
    /// </summary>
    /// <returns>The normalized vector.</returns>
    public Vector3 GetGravityDirection()
    {
        if (fields.Count > 0)
        {
            GravityField currentField = fields[0];
            Vector3 dir = currentField.getGravityDirection(this.transform.position);
            return dir;
        } 
        else
        {
            return Vector3.zero;
        }
    }

    private void OnValidate()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}
