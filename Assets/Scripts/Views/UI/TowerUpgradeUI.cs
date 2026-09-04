using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text levelText;

    [SerializeField]
    private TMP_Text attackPowerText;

    [SerializeField]
    private TMP_Text attackRangeText;

    [SerializeField]
    private TMP_Text upgradeCostText;

    private TowerController selectedTower;

    [SerializeField]
    private Button upgradeButton;

    public void Show(TowerController tower)
    {
        selectedTower = tower;

        UpdateUI();

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        selectedTower = null;
        gameObject.SetActive(false);
    }

    public void OnUpgradeButton()
    {
        if (selectedTower == null)
        {
            return;
        }

        selectedTower.Upgrade();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (selectedTower == null)
        {
            return;
        }

        TowerModel model = selectedTower.GetModel();

        levelText.text =
            "Level: " + model.Level;

        attackPowerText.text =
            "Attack: " + model.AttackPower;

        attackRangeText.text =
            "Range: " + model.AttackRange;

        upgradeCostText.text =
            "Cost: " + model.UpgradeCost;

        upgradeButton.interactable =
            model.Level < 5;
    }
}