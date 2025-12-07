using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton paliktas dėl Unity scenos valdymo / Service Locator funkcijos
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    // Inkapsuliacija: privatūs laukai su getteriais/viešais metodais
    private int _currency;
    public int PlayerLives { get; private set; }
    public int WavesDefeated { get; private set; }
    public int MaxWaves { get; private set; } = GameConstants.MAX_WAVES;
    private bool isGameOver = false;

    private void Awake()
    {
        main = this;
        PlayerLives = GameConstants.START_LIVES;
        _currency = GameConstants.START_CURRENCY;
    }

    // Inkapsuliacija: getteris
    public int GetCurrency() => _currency;
    public bool IsGameActive() => !isGameOver;

    public void IncreaseCurrency(int amount)
    {
        _currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= _currency)
        {
            _currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough money!!!");
            return false;
        }
    }

    public void EnemyReachedEndpoint()
    {
        if (isGameOver) return;

        PlayerLives--;
        Debug.Log("Lives left: " + PlayerLives);

        if (PlayerLives <= 0)
        {
            GameOver();
        }
    }

    public void WaveCompleted()
    {
        WavesDefeated++;
        Debug.Log("Wave Completed! Total waves defeated: " + WavesDefeated);

        if (WavesDefeated >= MaxWaves)
        {
            Victory();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! You lost all your lives.");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = GameConstants.TIME_SCALE_PAUSED;
        }
    }

    private void Victory()
    {
        isGameOver = true;
        Debug.Log("You defended The Garden!");
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = GameConstants.TIME_SCALE_PAUSED;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = GameConstants.TIME_SCALE_NORMAL;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}