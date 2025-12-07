using UnityEngine;

// Paveldėjimas: Perima TakeDamage() elgesį iš Damageable
public class EnemyHealth : Damageable
{
    [SerializeField]
    private int currencyWorth = 50;

    // Perrašomas abstraktus metodas
    protected override void Die()
    {
        // Observer Pattern: pranešimas spawner'iui
        EnemySpawner.onEnemyDestroy.Invoke();
        LevelManager.main.IncreaseCurrency(currencyWorth);

        isDestroyed = true;

        Destroy(gameObject);
    }
}