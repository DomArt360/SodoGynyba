using UnityEngine;
// UnityEditor nereikalingas, jei nenaudojamas Handles.DrawWireDisc ne redaktoriaus režime
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Turret : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f;

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (!LevelManager.main.IsGameActive()) return;

        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            RotateTowardsTarget();
            TryShoot();
        }
    }

    // Švarus kodas: atskirta šaudymo logika
    private void TryShoot()
    {
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / bps)
        {
            Shoot();
            timeUntilFire = 0f;
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletprefab, firingPoint.position, Quaternion.identity);

        // Patikrinimas, kad užtikrinti saugumą
        if (bulletObj.TryGetComponent(out Bullet bulletScript))
        {
            bulletScript.SetTarget(target);
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Nereikalingas UnityEditor importas, jei šis blokas pašalinamas build'inant
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        // Naudojama Graphics klasė vietoj Handles, jei norima matyti žaidimo režime
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}