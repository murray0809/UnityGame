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

    [SerializeField]
    private TextMeshProUGUI resultText;   // Å© í«â¡

    private void Awake()
    {
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }
    }

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

    // "GAME OVER" / "GAME CLEAR" ÇâÊñ Ç…ï\é¶Ç∑ÇÈ
    public void ShowResult(string message)
    {
        if (resultText == null)
        {
            return;
        }

        resultText.text = message;
        resultText.gameObject.SetActive(true);
    }
}