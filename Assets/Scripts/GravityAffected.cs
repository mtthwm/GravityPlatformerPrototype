using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityAffected : MonoBehaviour
{
    LinkedList<GravityField> fields = new LinkedList<GravityField>();
    Rigidbody rb;

    private void OnValidate()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        Debug.Log(fields);
        if (fields.Count > 0)
        {
            GravityField currentField = fields.First.Value;
            Vector3 dir = currentField.getGravityDirection(this.transform.position);
            Debug.Log(dir);
            rb.AddForce(dir * Physics.gravity.y);

            transform.up = dir;
        }
    }

    public void AddAffectingField (GravityField field)
    {
        fields.AddLast(field);
    }

    public void RemoveAffectingField (GravityField field)
    {
        fields.Remove(field);
    }
}
