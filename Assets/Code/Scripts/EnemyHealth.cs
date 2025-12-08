using UnityEngine;

public class EnemyHealth : Damageable
{
    [SerializeField]
    private int currencyWorth = 50;

    protected override void Die()
    {
        EnemySpawner.onEnemyDestroy.Invoke();
        LevelManager.main.IncreaseCurrency(currencyWorth);

        isDestroyed = true;

        Destroy(gameObject);
    }
}