using UnityEngine;
using UnityEngine.Events;

public abstract class Damageable : MonoBehaviour
{
    // Inkapsuliacija: protected laukai
    [Header("Attributes")]
    [SerializeField]
    protected int maxHitPoints = 2;

    protected int currentHitPoints;
    protected bool isDestroyed = false;

    // Observer Pattern: įvykis, kurį kviečia mirtis
    public UnityEvent OnDeath = new UnityEvent();

    protected virtual void Start()
    {
        currentHitPoints = maxHitPoints;
    }

    // Polimorfizmo bazinis metodas
    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        currentHitPoints -= damage;

        if (currentHitPoints <= 0)
        {
            Die();
        }
    }

    // Abstraktus metodas – turi būti įgyvendintas paveldinčioje klasėje
    protected abstract void Die();

    // Inkapsuliacija: getteris
    public int GetCurrentHealth() => currentHitPoints;
}