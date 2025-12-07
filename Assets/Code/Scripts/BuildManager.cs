using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    // Singleton paliktas dėl Unity scenos valdymo
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    [Header("UI Buttons")]
    [SerializeField] private Button[] towerButtons;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color normalColor = Color.white;

    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        InitializeButtons();
    }

    // Švarus kodas: atskirta Start() logika
    private void InitializeButtons()
    {
        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i].image.color = normalColor;
        }
        if (towerButtons.Length > 0)
            towerButtons[selectedTower].image.color = selectedColor;
    }

    // Inkapsuliacija: getteris
    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    // Švarus kodas: atskirta UI atnaujinimo logika
    public void SetSelectedTower(int _selectedTower)
    {
        UpdateOldSelectionColor();
        selectedTower = _selectedTower;
        UpdateNewSelectionColor();
    }

    private void UpdateOldSelectionColor()
    {
        if (selectedTower >= 0 && selectedTower < towerButtons.Length)
        {
            towerButtons[selectedTower].image.color = normalColor;
        }
    }

    private void UpdateNewSelectionColor()
    {
        if (selectedTower >= 0 && selectedTower < towerButtons.Length)
        {
            towerButtons[selectedTower].image.color = selectedColor;
        }
    }
}