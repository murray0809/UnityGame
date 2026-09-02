using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lifeText;

    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private TextMeshProUGUI waveText;

    public void UpdateLife(int life)
    {
        lifeText.text = "Life: " + life;
    }

    public void UpdateMoney(int money)
    {
        moneyText.text = "Money: " + money;
    }

    public void UpdateWave(int currentWave, int totalWaves)
    {
        waveText.text =
            "Wave: " + currentWave + " / " + totalWaves;
    }
}