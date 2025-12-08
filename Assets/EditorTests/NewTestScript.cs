using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;


public class MockDamageable
{
    private int _currentHitPoints;
    private bool _isDead = false;
    public bool DidDieCall = false;

    public MockDamageable(int health) => _currentHitPoints = health;

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHitPoints -= damage;

        if (_currentHitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DidDieCall = true;
        _isDead = true;
    }

    public int GetCurrentHealth() => _currentHitPoints;
}

public class MockLevelManager
{
    private int _currency;
    public int PlayerLives { get; private set; }

    public MockLevelManager(int currency, int lives)
    {
        _currency = currency;
        PlayerLives = lives;
    }

    public int GetCurrency() => _currency;

    public void IncreaseCurrency(int amount) => _currency += amount;

    public bool SpendCurrency(int amount)
    {
        if (amount <= _currency)
        {
            _currency -= amount;
            return true;
        }
        return false;
    }

    public void EnemyReachedEndpoint()
    {
        PlayerLives--;
    }
}


// ==========================================================
// UNIT TESTAI
// ==========================================================

[TestFixture] 
public class GameLogicTests
{
    // 1. Tikrina, ar teisingai mažinamos gyvybės po žalos (Damageable)
    [Test]
    [TestCase(10, 3, 7)]
    [TestCase(5, 5, 0)]  
    public void Test_1_TakeDamage_ReducesHealthCorrectly(int initialHealth, int damage, int expectedHealth)
    {
        
        var damageable = new MockDamageable(initialHealth);

        
        damageable.TakeDamage(damage);

        Assert.AreEqual(expectedHealth, damageable.GetCurrentHealth(), "Gyvybės turėtų būti sumažintos teisingu skaičiumi.");
    }

    
    [Test]
    public void Test_2_TakeDamage_ZeroHealth_InvokesDie()
    {
        
        var damageable = new MockDamageable(5);

        
        damageable.TakeDamage(5);

        
        Assert.IsTrue(damageable.DidDieCall, "Mirties metodas (Die) turėjo būti iškviestas pasiekus 0 HP.");
    }

   
    [Test]
    public void Test_3_SpendCurrency_SufficientFunds_ReturnsTrueAndReducesBalance()
    {
        var manager = new MockLevelManager(200, 3);
        int cost = 50;

        bool result = manager.SpendCurrency(cost);

        Assert.IsTrue(result, "SpendCurrency turėtų grąžinti true.");
        Assert.AreEqual(150, manager.GetCurrency(), "Valiuta turėjo sumažėti po išleidimo.");
    }

    [Test]
    public void Test_4_SpendCurrency_InsufficientFunds_ReturnsFalseAndKeepsBalance()
    {
        var manager = new MockLevelManager(10, 3);
        int cost = 50;

        bool result = manager.SpendCurrency(cost);

        Assert.IsFalse(result, "SpendCurrency turėtų grąžinti false.");
        Assert.AreEqual(10, manager.GetCurrency(), "Valiuta neturėtų keistis.");
    }

    [Test]
    public void Test_5_IncreaseCurrency_IncreasesBalanceCorrectly()
    {
        var manager = new MockLevelManager(100, 3);
        int amount = 75;

        manager.IncreaseCurrency(amount);

        Assert.AreEqual(175, manager.GetCurrency(), "Valiuta turėjo padidėti teisinga suma.");
    }

   
    [Test]
    public void Test_6_EnemyReachedEndpoint_ReducesPlayerLives()
    {
       
        var manager = new MockLevelManager(100, 3);

      
        manager.EnemyReachedEndpoint();

        
        Assert.AreEqual(2, manager.PlayerLives, "Žaidėjo gyvybės turėjo sumažėti vienu tašku.");
    }

   
    [Test]
    [TestCase(1, 8)]  
    [TestCase(5, 27)] 
    [TestCase(10, 45)]
    public void Test_7_EnemiesPerWave_CalculatesScalingCorrectly(int waveNumber, int expectedEnemies)
    {
        float baseEnemies = 8f;
        float difficultyScalingFactor = 0.75f;

        int result = Mathf.RoundToInt(baseEnemies * Mathf.Pow(waveNumber, difficultyScalingFactor));

        Assert.AreEqual(expectedEnemies, result, $"Bangos {waveNumber} priešų skaičius apskaičiuotas neteisingai.");
    }
}