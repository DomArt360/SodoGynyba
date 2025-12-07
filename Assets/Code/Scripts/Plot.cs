using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (CanBuildTower(towerToBuild))
        {
            BuildTower(towerToBuild);
        }
    }

    // Švarus kodas: logika padalinta
    private bool CanBuildTower(Tower towerToBuild)
    {
        if (towerToBuild.cost > LevelManager.main.GetCurrency())
        {
            Debug.Log("You can't afford this tower");
            return false;
        }
        return true;
    }

    // Švarus kodas: logika padalinta
    private void BuildTower(Tower towerToBuild)
    {
        if (LevelManager.main.SpendCurrency(towerToBuild.cost))
        {
            tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        }
    }
}