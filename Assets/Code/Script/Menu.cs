using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] private TextMeshProUGUI livesUI;
    [SerializeField] private TextMeshProUGUI waveUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void OnGUI()
    {
        if (currencyUI != null)
            currencyUI.text = "Money: " + LevelManager.main.currency;

        if (livesUI != null)
            livesUI.text = "Lives: " + LevelManager.main.playerLives;

        if (waveUI != null)
            waveUI.text = "Waves completed: " + LevelManager.main.wavesDefeated;
    }

    public void SetSelected()
    {

    }
}
