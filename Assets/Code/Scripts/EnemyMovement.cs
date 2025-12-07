using UnityEngine;

// Interfeiso realizacija
public class EnemyMovement : MonoBehaviour, IMovement
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    // Inkapsuliacija: getteris
    public float MoveSpeed => moveSpeed;

    private void Start()
    {
        if (LevelManager.main == null || LevelManager.main.path == null || LevelManager.main.path.Length == 0)
        {
            Debug.LogError("LevelManager or path not initialized!");
            enabled = false;
            return;
        }
        pathIndex = 0;
        target = LevelManager.main.path[pathIndex];
        transform.position = target.position;
    }

    private void Update()
    {
        if (target == null) return;
        CheckIfTargetReached();
    }

    // Švarus kodas: didelė logika padalinta į mažesnį metodą
    private void CheckIfTargetReached()
    {
        if (Vector2.Distance(transform.position, target.position) <= GameConstants.ENEMY_REACHED_DISTANCE)
        {
            pathIndex++;
            if (pathIndex >= LevelManager.main.path.Length)
            {
                LevelManager.main.EnemyReachedEndpoint();
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        FixedUpdateMovement(target);
    }

    // Interfeiso metodo realizacija
    public void FixedUpdateMovement(Transform target)
    {
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
}