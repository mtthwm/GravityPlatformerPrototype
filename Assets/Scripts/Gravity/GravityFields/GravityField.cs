using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GravityField : MonoBehaviour
{
    public abstract Vector3 getGravityDirection(Vector3 otherPosition);

    private void OnTriggerEnter(Collider other)
    {
        GravityAffected affected = other.transform.root.GetComponent<GravityAffected>();
        if (affected != null)
        {
            affected.AddAffectingField(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GravityAffected affected = other.transform.root.GetComponent<GravityAffected>();
        if (affected != null)
        {
            affected.RemoveAffectingField(this);
        }
    }
}
