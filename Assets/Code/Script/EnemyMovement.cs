using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        if (LevelManager.main == null || LevelManager.main.path == null || LevelManager.main.path.Length == 0)
        {
            Debug.LogError("LevelManager or path not initialized!");
            enabled = false;  // Disable this script to avoid errors
            return;
        }

        pathIndex = 0;
        target = LevelManager.main.path[pathIndex];
        transform.position = target.position;  // Optionally start at first path point
    }

    private void Update()
    {
        if (target == null) return;

        // Check distance to current target
        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            pathIndex++;

            if (LevelManager.main.path == null || pathIndex >= LevelManager.main.path.Length)
            {
                Debug.Log("Enemy reached endpoint - reducing life!");
                LevelManager.main.EnemyReachedEndpoint();
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
}
