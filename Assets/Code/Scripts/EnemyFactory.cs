using UnityEngine;

public static class EnemyFactory
{
    public static GameObject CreateEnemy(GameObject prefab, Transform spawnPoint)
    {
        if (prefab == null || spawnPoint == null)
        {
            Debug.LogError("EnemyFactory: Null prefab or spawn point provided.");
            return null;
        }

        return Object.Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}