using NUnit.Framework;
using UnityEngine; // Reikalingas Mathf
using System;
using System.Collections.Generic;

// ==========================================================
// MOCK KLASĖS (Simuliuoja LevelManager ir Damageable)
// Reikalingos, nes negalime tiesiogiai instancijuoti 
// MonoBehaviour klasių Unit Testuose
// ==========================================================

public class MockDamageable
{
    private int _currentHitPoints;
    private bool _isDead = false;
    public bool DidDieCall = false; // Stebima, ar buvo iškviestas Die()

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

[TestFixture] // Nurodo klasei, kad ji talpina testus
public class GameLogicTests
{
    // 1. Tikrina, ar teisingai mažinamos gyvybės po žalos (Damageable)
    [Test]
    [TestCase(10, 3, 7)] // Pradinė 10, žala 3, likę 7
    [TestCase(5, 5, 0)]  // Pradinė 5, žala 5, likę 0
    public void Test_1_TakeDamage_ReducesHealthCorrectly(int initialHealth, int damage, int expectedHealth)
    {
        // ARRANGE
        var damageable = new MockDamageable(initialHealth);

        // ACT
        damageable.TakeDamage(damage);

        // ASSERT (Tikrina objekto būsenos pasikeitimą)
        Assert.AreEqual(expectedHealth, damageable.GetCurrentHealth(), "Gyvybės turėtų būti sumažintos teisingu skaičiumi.");
    }

    // 2. Tikrina, ar iškviečiamas mirties metodas (Die()), kai gyvybės pasiekia nulį
    [Test]
    public void Test_2_TakeDamage_ZeroHealth_InvokesDie()
    {
        // ARRANGE
        var damageable = new MockDamageable(5);

        // ACT
        damageable.TakeDamage(5);

        // ASSERT (Tikrina objekto būsenos/metodo iškvietimą)
        Assert.IsTrue(damageable.DidDieCall, "Mirties metodas (Die) turėjo būti iškviestas pasiekus 0 HP.");
    }

    // 3. Tikrina, ar valiuta išleidžiama sėkmingai, kai lėšų užtenka
    [Test]
    public void Test_3_SpendCurrency_SufficientFunds_ReturnsTrueAndReducesBalance()
    {
        // ARRANGE
        var manager = new MockLevelManager(200, 3);
        int cost = 50;

        // ACT
        bool result = manager.SpendCurrency(cost);

        // ASSERT (Tikrina skaičiavimus ir būsenos pasikeitimą)
        Assert.IsTrue(result, "SpendCurrency turėtų grąžinti true.");
        Assert.AreEqual(150, manager.GetCurrency(), "Valiuta turėjo sumažėti po išleidimo.");
    }

    // 4. Tikrina, ar valiuta neišleidžiama, kai lėšų trūksta
    [Test]
    public void Test_4_SpendCurrency_InsufficientFunds_ReturnsFalseAndKeepsBalance()
    {
        // ARRANGE
        var manager = new MockLevelManager(10, 3);
        int cost = 50;

        // ACT
        bool result = manager.SpendCurrency(cost);

        // ASSERT (Tikrina skaičiavimus ir būsenos nepakitimą)
        Assert.IsFalse(result, "SpendCurrency turėtų grąžinti false.");
        Assert.AreEqual(10, manager.GetCurrency(), "Valiuta neturėtų keistis.");
    }

    // 5. Tikrina, ar teisingai padidinamas valiutos kiekis
    [Test]
    public void Test_5_IncreaseCurrency_IncreasesBalanceCorrectly()
    {
        // ARRANGE
        var manager = new MockLevelManager(100, 3);
        int amount = 75;

        // ACT
        manager.IncreaseCurrency(amount);

        // ASSERT (Tikrina skaičiavimus)
        Assert.AreEqual(175, manager.GetCurrency(), "Valiuta turėjo padidėti teisinga suma.");
    }

    // 6. Tikrina sąveiką: ar sumažėja žaidėjo gyvybės, kai priešas pasiekia pabaigos tašką
    [Test]
    public void Test_6_EnemyReachedEndpoint_ReducesPlayerLives()
    {
        // ARRANGE
        var manager = new MockLevelManager(100, 3);

        // ACT
        manager.EnemyReachedEndpoint();

        // ASSERT (Tikrina sąveiką tarp objektų būsenų)
        Assert.AreEqual(2, manager.PlayerLives, "Žaidėjo gyvybės turėjo sumažėti vienu tašku.");
    }

    // 7. Tikrina bangų skaičiavimo formulę (sudėtingas skaičiavimas)
    [Test]
    [TestCase(1, 8)]  // 1 banga
    [TestCase(5, 27)] // 5 banga (8 * 5^0.75 = 26.83, suapvalinta)
    [TestCase(10, 45)]// 10 banga (8 * 10^0.75 = 44.97, suapvalinta)
    public void Test_7_EnemiesPerWave_CalculatesScalingCorrectly(int waveNumber, int expectedEnemies)
    {
        // ARRANGE (Iš EnemySpawner.cs)
        float baseEnemies = 8f;
        float difficultyScalingFactor = 0.75f;

        // ACT
        int result = Mathf.RoundToInt(baseEnemies * Mathf.Pow(waveNumber, difficultyScalingFactor));

        // ASSERT (Tikrina skaičiavimus pagal sudėtingą formulę)
        Assert.AreEqual(expectedEnemies, result, $"Bangos {waveNumber} priešų skaičius apskaičiuotas neteisingai.");
    }
}