using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    [Header("UI Buttons")]
    [SerializeField] private Button[] towerButtons;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color normalColor = Color.white;

    private int selectedTower = 0;

    private void Start()
    {
        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i].image.color = normalColor;
        }

        if (towerButtons.Length > 0)
            towerButtons[selectedTower].image.color = selectedColor;
    }

    private void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public void SetSelectedTower(int _selectedTower)
    {
        if (selectedTower >= 0 && selectedTower < towerButtons.Length)
        {
            towerButtons[selectedTower].image.color = normalColor;
        }

        selectedTower = _selectedTower;

        if (selectedTower >= 0 && selectedTower < towerButtons.Length)
        {
            towerButtons[selectedTower].image.color = selectedColor;
        }
    }
}
