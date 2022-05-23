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
    [SerializeField] float spread = 45f;
    [SerializeField] int capacity = 10; // NOT USED
    [SerializeField] int bulletCount = 1;
    [SerializeField] float range = 50f;
    [SerializeField] float trailPersistTime = 0.05f;
    [SerializeField] LayerMask layers;
    [SerializeField] GunTypes type = GunTypes.Automatic;
    [SerializeField] GameObject bulletTrailPrefab;

    Transform origin;
    Transform cameraTransform;
    int continuousShots;

    Vector3 debug_target;

    private void Start()
    {
        origin = transform.Find("Origin");
        cameraTransform = Camera.main.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(debug_target, 0.1f);
    }

    public virtual void BeginShoot ()
    {
        StartCoroutine("ShootCoroutine");
    }

    public virtual void EndShoot()
    {
        StopCoroutine("ShootCoroutine");
    }

    private IEnumerator ShootCoroutine ()
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

        for (int i = 0; i < bulletCount; i++)
        {
            HandleShot(target);
        }
    }

    protected virtual void HandleShot (Vector3 target)
    {
        RaycastHit hit;
        Vector3 direction = ObtainDirection(target);
        bool hasHit = Physics.Raycast(origin.position, direction, out hit, range, layers, QueryTriggerInteraction.Ignore);

        if (hasHit)
        {
            ResolveHit(hit);
            StartCoroutine(BulletTrailCoroutine(origin.position, hit.point, trailPersistTime));
        }
        else
        {
            StartCoroutine(BulletTrailCoroutine(origin.position, origin.position + (direction * range), trailPersistTime));
        }
    }

    protected virtual Vector3 ObtainDirection (Vector3 target)
    {
        Vector3 upVector = new Vector3(Random.Range(-spread, spread), 1f, Random.Range(-spread, spread));
        Quaternion upToTarget = Quaternion.FromToRotation(Vector3.up, target - origin.position);
        Vector3 final = upToTarget * upVector;
        final = final.normalized;
        return final;
    }

    protected virtual void ResolveHit (RaycastHit hit)
    {
        //DO SOMETHING
        Debug.Log("hit!");
    }

    private IEnumerator BulletTrailCoroutine (Vector3 start, Vector3 end, float time)
    {
        LineRenderer trail = Instantiate(bulletTrailPrefab).GetComponent<LineRenderer>(); ;
        trail.SetPosition(0, start);
        trail.SetPosition(1, end);

        yield return new WaitForSeconds(time);

        Destroy(trail.gameObject);

    }

    private Vector3 ObtainTarget ()
    {
        Vector3 baseDirection = cameraTransform.position + (cameraTransform.forward.normalized * range);
        return baseDirection;
    }
}
