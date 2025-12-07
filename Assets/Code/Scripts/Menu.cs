using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private TextMeshProUGUI livesUI;
    [SerializeField] private TextMeshProUGUI waveUI;
    [SerializeField] private Animator anim;

    private bool isMenuOpen = true;
    private LevelManager levelManager; // Cache reference

    private void Start()
    {
        levelManager = LevelManager.main;
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }

    private void Update()
    {
        if (levelManager == null) return;

        UpdateUIElements();
    }

    // Švarus kodas: atskirta UI atnaujinimo logika
    private void UpdateUIElements()
    {
        if (currencyUI != null) currencyUI.text = "Money: " + levelManager.GetCurrency();
        if (livesUI != null) livesUI.text = "Lives: " + levelManager.PlayerLives;
        if (waveUI != null) waveUI.text = "Waves completed: " + levelManager.WavesDefeated;
    }

    public void SetSelected() { }
}