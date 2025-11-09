using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public int currency;
    public int playerLives = 2;
    public int wavesDefeated = 0;

    public GameObject gameOverPanel;
    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
    }
    public void IncreaseCurrency( int amount)
    {
        currency += amount;
    }
    public bool SpendCurrency( int amount )
    {
        if ( amount <= currency ) {
            currency -= amount;
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
        playerLives--;
        Debug.Log("Lives left: " + playerLives);

        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void WaveCompleted()
    {
        wavesDefeated++;
        Debug.Log("Wave Completed! Total waves defeated: " + wavesDefeated);
    }

    private void GameOver()
    {
        Debug.Log("Game Over! You lost all your lives.");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
