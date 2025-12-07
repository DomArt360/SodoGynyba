using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    // Observer Pattern: Publisher
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; // Enemies Per Second

    private void Awake()
    {
        // Observer Pattern: Subscriber
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!LevelManager.main.IsGameActive()) return;
        if (!isSpawning()) return;

        timeSinceLastSpawn += Time.deltaTime;

        // Švarus kodas: atskirta spawn'inimo logika
        TrySpawnEnemy();

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private bool isSpawning() => enemiesLeftToSpawn > 0;

    private void TrySpawnEnemy()
    {
        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        timeSinceLastSpawn = 0f;
        LevelManager.main.WaveCompleted();
        currentWave++;
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int maxEnemyIndex = GetMaxEnemyIndexForWave();
        int index = Random.Range(0, maxEnemyIndex + 1);
        GameObject prefabToSpawn = enemyPrefabs[index];

        // Factory Method naudojimas
        GameObject newEnemy = EnemyFactory.CreateEnemy(prefabToSpawn, LevelManager.main.startPoint);

        if (newEnemy != null)
        {
            enemiesLeftToSpawn--;
            enemiesAlive++;
        }
    }

    // Švarus kodas: metodai mažesni ir atsakingi už vieną dalyką
    private int GetMaxEnemyIndexForWave()
    {
        // Galima refaktorizuoti į ScriptableObject ateityje
        if (currentWave <= 1) return 1;
        else if (currentWave == 2) return 2;
        else if (currentWave == 3) return 3;
        else return 4;
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), GameConstants.MIN_EPS, GameConstants.MAX_EPS);
    }
}