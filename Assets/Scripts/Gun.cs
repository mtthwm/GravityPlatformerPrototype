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
        bool hasHit = Physics.Raycast(origin.position, ObtainDirection(target), out hit, range, layers, QueryTriggerInteraction.Ignore);

        if (hasHit)
        {
            ResolveHit(hit);
            StartCoroutine(BulletTrailCoroutine(origin.position, hit.point, trailPersistTime));
        }
        else
        {
            StartCoroutine(BulletTrailCoroutine(origin.position, target, trailPersistTime));
        }
    }

    protected virtual Vector3 ObtainDirection (Vector3 target)
    {
        Debug.Log("ObtainDirection");
        Vector3 randomlyRotatedVector = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Vector3.right;
        Vector3 adjustedRandomlyRotatedVector = Quaternion.FromToRotation(Vector3.up, (target - origin.position).normalized) * randomlyRotatedVector;
        float randomRotation = Random.Range(-spread, spread);
        Debug.Log(randomRotation + " ");
        Quaternion finalRotation = Quaternion.AngleAxis(randomRotation, adjustedRandomlyRotatedVector);
        return finalRotation * (target - origin.position).normalized;
    }

    protected virtual void ResolveHit (RaycastHit hit)
    {
        //DO SOMETHING
        Debug.Log("hit!");
    }

    private IEnumerator BulletTrailCoroutine (Vector3 start, Vector3 end, float time)
    {
        GameObject trailRenderer = Instantiate(bulletTrailPrefab);
        LineRenderer line = trailRenderer.GetComponent<LineRenderer>();
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(time);

        Destroy(trailRenderer);
    }

    private Vector3 ObtainTarget ()
    {
        Vector3 baseDirection = cameraTransform.position + (cameraTransform.forward.normalized * range);
        return baseDirection;
    }
}
