using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunTypes
    {
        Automatic,
        Manual,
    }

    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float reloadSpeed = 1f;
    [SerializeField] float spread = 0.1f;
    [SerializeField] int capacity = 10; // NOT USED
    [SerializeField] int bulletCount = 1;
    [SerializeField] float range = 50f;
    [SerializeField] LayerMask layers;
    [SerializeField] GunTypes type = GunTypes.Automatic;

    Transform origin;
    Transform cameraTransform;
    int continuousShots;
    bool isShooting;

    Vector3 debug_target;

    private void Start()
    {
        origin = transform.Find("Origin");
        cameraTransform = Camera.main.transform;
    }

    public void BeginShoot ()
    {
        StartCoroutine("HandleShoot");
    }

    public void EndShoot ()
    {
        StopCoroutine("HandleShoot");
    }

    private IEnumerator HandleShoot ()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Shoot()
    {
        if (type == GunTypes.Manual && continuousShots > 0)
        {
            return;
        }

        Vector3 target = ObtainTarget();

        debug_target = target;

    }

    private Vector3 ObtainTarget ()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward.normalized, out hit, range, layers, QueryTriggerInteraction.Ignore);
        Vector3 target;

        if (hasHit)
        {
            target = hit.point;

        }
        else
        {
            target = cameraTransform.position + (cameraTransform.forward.normalized * range);
        }

        return target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(debug_target, 0.1f);
    }
}
